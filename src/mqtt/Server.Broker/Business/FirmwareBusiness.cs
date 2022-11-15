using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using Server.Broker.Hubs;
using Server.Broker.Infrastructure.Entities;
using Server.Broker.Interfaces;
using Service.Shared.Contracts.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Broker.Business
{
    public class FirmwareBusiness : IFirmwareBusiness
    {
        private readonly IMqttServer mqttServer;

        private readonly ILogger<FirmwareBusiness> logger;
        private readonly IFirmwareVersionRepository firmwareVersionRepository;
        private readonly IFirmwareRepository firmwareRepository;
        private readonly IHubContext<DeviceHub> hubContext;

        public FirmwareBusiness(ILogger<FirmwareBusiness> logger, IFirmwareVersionRepository firmwareVersionRepository, IFirmwareRepository firmwareRepository, IMqttServer mqttServer, IHubContext<DeviceHub> hubContext)
        {
            this.logger = logger;
            this.firmwareVersionRepository = firmwareVersionRepository;
            this.firmwareRepository = firmwareRepository;
            this.mqttServer = mqttServer;
            this.hubContext = hubContext;
        }

        public async Task<IEnumerable<FirmwareItemResponse>> GetFirmwaresAsync()
        {
            var firmwaresList = new List<FirmwareItemResponse>();
            var firmwares = await firmwareRepository.QueryMultipleAsync(new Firmware() { });

            foreach (var firmware in firmwares)
            {
                var versions = await firmwareVersionRepository.QueryMultipleAsync(new FirmwareVersion()
                {
                    FirmwareId = firmware.Id
                });

                var lastVersion = versions.OrderBy(o => o.CreateAt).LastOrDefault();

                var firmwareResponse = new FirmwareItemResponse()
                {
                    Id = firmware.Id,
                    Name = firmware.Name,
                    Description = firmware.Description,
                    Version = lastVersion?.Version,
                    LastVersionDate = lastVersion?.CreateAt
                };

                firmwaresList.Add(firmwareResponse);
            }

            return firmwaresList.ToArray();
        }


        public async Task<bool> InstallOnDeviceAsync(string deviceName, Guid firmwareId)
        {
            var publishMessage = await mqttServer.PublishAsync(new MQTTnet.MqttApplicationMessage()
            {
                Topic = $"mqtt/device/{deviceName}/update",
                Payload = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new
                {
                    uri = $"http://192.168.1.131:5001/api/firmware/download/{firmwareId}.bin"
                })),
                QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce
            });

            return publishMessage.ReasonCode == MQTTnet.Client.Publishing.MqttClientPublishReasonCode.Success;
        }

        public Task<FirmwareVersion> GetFirmwareVersionAsync(Guid id)
        {
            return firmwareVersionRepository.QueryOneAsync(new FirmwareVersion() { Id = id });
        }

        public async Task<IEnumerable<FirmwareVersionItemResponse>> GetFirmwareVersionsAsync(Guid firmwareId)
        {
            var version = await firmwareVersionRepository.QueryMultipleAsync(new FirmwareVersion() { FirmwareId = firmwareId });
            return version.Select(s => new FirmwareVersionItemResponse() { Id = s.Id, Version = s.Version }).OrderBy(o => o.Version).ToArray();
        }

        public async Task<(bool success, bool isConflict)> UploadFileAsync(IFormFile file)
        {
            try
            {
                var firmwareDb = await firmwareRepository.QueryOneAsync(new Infrastructure.Entities.Firmware()
                {
                    Name = file.FileName
                });

                string checksumMd5 = null;
                using (var md5 = MD5.Create())
                {
                    checksumMd5 = BitConverter.ToString(md5.ComputeHash(file.OpenReadStream())).Replace("-", "").ToLowerInvariant();
                    logger.LogInformation($"Checksum MD5 {checksumMd5}");
                }

                if (firmwareDb == null)
                {
                    firmwareDb = new Infrastructure.Entities.Firmware()
                    {
                        Id = Guid.NewGuid(),
                        Name = file.FileName,
                        Description = file.FileName,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = "System"
                    };

                    if (await firmwareRepository.InsertAsync(firmwareDb) == null)
                    {
                        return (false, false);
                    }
                    else
                    {
                        var firmwareVersion = new Infrastructure.Entities.FirmwareVersion()
                        {
                            FirmwareId = firmwareDb.Id,
                            CreateAt = DateTime.UtcNow,
                            CreateBy = "System",
                            Content = ReadFully(file.OpenReadStream()),
                            Checksum = checksumMd5,
                            Version = 1
                        };

                        var insertResult = await firmwareVersionRepository.InsertAsync(firmwareVersion);

                        if (insertResult != null)
                            await hubContext.Clients.All.SendAsync("FirmwareUpdate");

                        return (insertResult != null, false);
                    }
                }
                else
                {
                    var firmwareVersions = await firmwareVersionRepository.QueryMultipleAsync(new Infrastructure.Entities.FirmwareVersion()
                    {
                        FirmwareId = firmwareDb.Id
                    });

                    if (firmwareVersions.Any(f => f.Checksum == checksumMd5))
                        return (false, true);

                    var lastVersion = firmwareVersions.Any() ? firmwareVersions.Max(m => m.Version) : 0;

                    var firmwareVersion = new Infrastructure.Entities.FirmwareVersion()
                    {
                        FirmwareId = firmwareDb.Id,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = "System",
                        Content = ReadFully(file.OpenReadStream()),
                        Checksum = checksumMd5,
                        Version = lastVersion + 1
                    };

                    var insertResult = await firmwareVersionRepository.InsertAsync(firmwareVersion);
                    if (insertResult != null)
                        await hubContext.Clients.All.SendAsync("FirmwareUpdate");
                    return (insertResult != null, false);
                }
            }
            catch (Exception)
            {
                return (false, false);
            }
        }

        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
