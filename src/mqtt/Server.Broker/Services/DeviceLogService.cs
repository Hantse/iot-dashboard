using Microsoft.Extensions.Logging;
using Server.Broker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Broker.Services
{
    public class DeviceLogService : IDeviceLogService
    {
        private readonly IDeviceLogRepository deviceLogRepository;
        private readonly IDeviceRepository deviceRepository;
        private readonly ILogger<DeviceLogService> logger;

        public static string DeviceConnection = "DeviceConnection";
        public static string DeviceDisconnection = "DeviceDisconnection";
        public static string DevicePing = "DevicePing";
        public static string DeviceSubscribe = "DeviceSubscribe";
        public static string DeviceUnsubscribe = "DeviceUnsubscribe";
        public static string DeviceMessage = "DeviceMessage";
        public static string DeviceFile = "DeviceFile";

        private const int MaxRetry = 5;

        private static Dictionary<string, string> ConnectedDevice = new Dictionary<string, string>();

        public DeviceLogService(IDeviceLogRepository deviceLogRepository, ILogger<DeviceLogService> logger, IDeviceRepository deviceRepository)
        {
            this.deviceLogRepository = deviceLogRepository;
            this.logger = logger;
            this.deviceRepository = deviceRepository;
        }

        public async Task LogConnection(string deviceId)
        {
            Infrastructure.Entities.Device device = null;
            int currentRetry = 0;

            do
            {
                device = await deviceRepository.QueryOneAsync(new Infrastructure.Entities.Device()
                {
                    Name = deviceId
                });

                if (device == null)
                {
                    await Task.Delay(5000);
                    currentRetry += 1;
                }
            } while (device == null && currentRetry < MaxRetry);

            if (device == null)
            {
                logger.LogInformation($"Unable to found device, event not log for {deviceId}");
                return;
            }

            var routineKey = Guid.NewGuid().ToString("N");
            await deviceLogRepository.InsertAsync(new Infrastructure.Entities.DeviceLog()
            {
                RoutineKey = routineKey,
                DeviceId = device.Id,
                Type = DeviceConnection,
                CreateAt = DateTime.UtcNow,
                CreateBy = deviceId
            });

            ConnectedDevice.Add(deviceId, routineKey);
        }

        public async Task LogDisconnection(string deviceId)
        {
            var deviceInformations = await GetDeviceRoutineKeyAsync(deviceId);

            await deviceLogRepository.InsertAsync(new Infrastructure.Entities.DeviceLog()
            {
                RoutineKey = deviceInformations.routineKey,
                DeviceId = deviceInformations.deviceId,
                Type = DeviceDisconnection,
                CreateAt = DateTime.UtcNow,
                CreateBy = deviceId
            });
        }

        public async Task LogFile(string deviceId)
        {
            var device = await deviceRepository.QueryOneAsync(new Infrastructure.Entities.Device()
            {
                Name = deviceId
            });

            if (device == null)
            {
                return;
            }

            var routineKey = ConnectedDevice[deviceId];
            await deviceLogRepository.InsertAsync(new Infrastructure.Entities.DeviceLog()
            {
                RoutineKey = routineKey,
                DeviceId = device.Id,
                Type = DeviceFile,
                CreateAt = DateTime.UtcNow,
                CreateBy = deviceId
            });
        }

        public async Task LogMessage(string deviceId, string message, string topic = null)
        {
            var deviceInformations = await GetDeviceRoutineKeyAsync(deviceId);
            await deviceLogRepository.InsertAsync(new Infrastructure.Entities.DeviceLog()
            {
                DeviceId = deviceInformations.deviceId,
                RoutineKey = deviceInformations.routineKey,
                Type = DeviceMessage,
                Content = $"Message:{message}#TopicSubscribe:{topic}",
                CreateAt = DateTime.UtcNow,
                CreateBy = deviceId
            });
        }

        public async Task LogSubscribe(string deviceId, string topic)
        {
            var deviceInformations = await GetDeviceRoutineKeyAsync(deviceId);
            await deviceLogRepository.InsertAsync(new Infrastructure.Entities.DeviceLog()
            {
                DeviceId = deviceInformations.deviceId,
                RoutineKey = deviceInformations.routineKey,
                Type = DeviceSubscribe,
                Content = $"TopicSubscribe:{topic}",
                CreateAt = DateTime.UtcNow,
                CreateBy = deviceId
            });
        }

        public async Task LogUnsubscribe(string deviceId, string topic)
        {
            var deviceInformations = await GetDeviceRoutineKeyAsync(deviceId);
            await deviceLogRepository.InsertAsync(new Infrastructure.Entities.DeviceLog()
            {
                DeviceId = deviceInformations.deviceId,
                RoutineKey = deviceInformations.routineKey,
                Type = DeviceUnsubscribe,
                Content = $"TopicUnsubscribe:{topic}",
                CreateAt = DateTime.UtcNow,
                CreateBy = deviceId
            });
        }

        public async Task LogPing(string deviceId)
        {
            var deviceInformations = await GetDeviceRoutineKeyAsync(deviceId);
            await deviceLogRepository.InsertAsync(new Infrastructure.Entities.DeviceLog()
            {
                DeviceId = deviceInformations.deviceId,
                RoutineKey = deviceInformations.routineKey,
                Type = DevicePing,
                CreateAt = DateTime.UtcNow,
                CreateBy = deviceId
            });
        }

        private async Task<(string routineKey, Guid deviceId)> GetDeviceRoutineKeyAsync(string deviceId)
        {
            string routineKey = null;
            var device = await deviceRepository.QueryOneAsync(new Infrastructure.Entities.Device()
            {
                Name = deviceId
            });

            if (device == null)
            {
                logger.LogInformation($"Unable to found device, event not log for {deviceId}");
                throw new ArgumentNullException("Device not found.");
            }

            if (ConnectedDevice.ContainsKey(deviceId))
            {
                routineKey = ConnectedDevice[deviceId];
            }
            else
            {
                var lastLogs = await deviceLogRepository.QueryMultipleByTypeAsync(device.Id, DeviceConnection);
                var lastConnectionState = lastLogs.OrderBy(o => o.CreateAt).Last();
                routineKey = lastConnectionState.RoutineKey;
            }

            return (routineKey, device.Id);
        }
    }
}
