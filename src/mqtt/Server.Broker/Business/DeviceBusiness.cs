using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using Server.Broker.Hubs;
using Server.Broker.Infrastructure.Entities;
using Server.Broker.Interfaces;
using Server.Broker.Services;
using Service.Shared.Contracts.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Broker.Business
{
	public class DeviceBusiness : IDeviceBusiness
	{
		private readonly IMqttServer mqttServer;
		private readonly ILogger<DeviceBusiness> logger;
		private readonly IDeviceRepository deviceRepository;
		private readonly IDeviceLogRepository logRepository;
		private readonly IGpsDataRepository gpsDataRepository;
		private readonly IDeviceFileRepository deviceFileRepository;
		private readonly IDeviceTopicSubscriptionRepository deviceTopicSubscriptionRepository;
		private readonly IHubContext<DeviceHub> hubContext;

		public DeviceBusiness(IMqttServer mqttServer, ILogger<DeviceBusiness> logger, IDeviceRepository deviceRepository, IDeviceLogRepository logRepository, IDeviceTopicSubscriptionRepository deviceTopicSubscriptionRepository, IHubContext<DeviceHub> hubContext, IDeviceFileRepository deviceFileRepository, IGpsDataRepository gpsDataRepository)
		{
			this.mqttServer = mqttServer;
			this.logger = logger;
			this.deviceRepository = deviceRepository;
			this.logRepository = logRepository;
			this.deviceTopicSubscriptionRepository = deviceTopicSubscriptionRepository;
			this.hubContext = hubContext;
			this.deviceFileRepository = deviceFileRepository;
			this.gpsDataRepository = gpsDataRepository;
		}

		public async Task<GpsDataResponse> GetDeviceLastPositionAsync(string deviceName)
		{
			var lastPosition = await gpsDataRepository.QueryLastOneAsync(new GpsData() { DeviceName = deviceName });
			return new GpsDataResponse()
			{
				Latitude = lastPosition.Latitude,
				Longitude = lastPosition.Longitude
			};
		}

		public async Task<List<GpsDataResponse>> GetDeviceAllPositionAsync(string deviceName)
		{
			var lastPositions = await gpsDataRepository.QueryMultipleAsync(new GpsData() { DeviceName = deviceName });
			TimeSpan interval = new TimeSpan(0, 2, 0);
			var positionGrouped = lastPositions.GroupBy(g => g.CreateAt.Ticks / interval.Ticks);

			return positionGrouped.Where(a => a.Any(c => c.Latitude != 0 && c.Longitude != 0))
									.Select(s => new
										GpsDataResponse()
									{
										Date = s.First().CreateAt,
										Latitude = s.First().Latitude,
										Longitude = s.First().Longitude
									}).ToList();
		}

		public async Task<bool> UpdateDeviceIpAsync(string deviceName, string ip)
		{
			var device = await deviceRepository.QueryOneAsync(new Infrastructure.Entities.Device()
			{
				Name = deviceName
			});

			if (device == null)
				return false;


			device.IP = ip;

			return await deviceRepository.UpdateAsync(device) > 0;
		}

		public async Task<bool> UpdateDeviceBoardInformationsAsync(string deviceName, string boardInformations)
		{
			logger.LogInformation($"Board info BL : {boardInformations}");
			var device = await deviceRepository.QueryOneAsync(new Device()
			{
				Name = deviceName
			});

			if (device == null)
				return false;

			device.BoardInformation = boardInformations;
			device.UpdateAt = DateTime.UtcNow;
			device.UpdateBy = "System";

			return await deviceRepository.UpdateAsync(device) > 0;
		}

		public async Task<DeviceItemResponse[]> GetDevicesAsync()
		{
			var devicesConnected = new List<DeviceItemResponse>();
			var devices = await deviceRepository.QueryMultipleAsync();
			var devicesInServer = await mqttServer.GetClientStatusAsync();

			foreach (var device in devices)
			{
				var deviceConnectionRoutines = await logRepository.CountByTypeAsync(new Infrastructure.Entities.DeviceLog() { DeviceId = device.Id, Type = DeviceLogService.DeviceConnection });
				var devicePingRoutines = await logRepository.CountByTypeAsync(new Infrastructure.Entities.DeviceLog() { DeviceId = device.Id, Type = DeviceLogService.DevicePing });
				var deviceSubscribeRoutines = await logRepository.CountByTypeAsync(new Infrastructure.Entities.DeviceLog() { DeviceId = device.Id, Type = DeviceLogService.DeviceSubscribe });
				var deviceUnsubscribeRoutines = await logRepository.CountByTypeAsync(new Infrastructure.Entities.DeviceLog() { DeviceId = device.Id, Type = DeviceLogService.DeviceUnsubscribe });
				var deviceMessageRoutines = await logRepository.CountByTypeAsync(new Infrastructure.Entities.DeviceLog() { DeviceId = device.Id, Type = DeviceLogService.DeviceMessage });

				var topics = await deviceTopicSubscriptionRepository.QueryMultipleAsync(new Infrastructure.Entities.DeviceTopicSubscription() { DeviceId = device.Id });

				var lastFile = await deviceFileRepository.QueryOneAsync(new Infrastructure.Entities.DeviceFile() { Name = device.Name });

				var deviceItem = new DeviceItemResponse()
				{
					Id = device.Id,
					Name = device.Name,
					Ip = device.IP,
					Online = devicesInServer.Any(a => a.ClientId == device.Name),
					ConnectionRoutineCount = deviceConnectionRoutines,
					PingRoutineCount = devicePingRoutines,
					SubscribeRoutineCount = deviceSubscribeRoutines,
					UnsubscribeRoutineCount = deviceUnsubscribeRoutines,
					MessageRoutineCount = deviceMessageRoutines,
					Topics = topics.Select(s => s.Topic).ToArray(),
					LastPicture = lastFile?.Data,
					BoardInformation = device.BoardInformation
				};

				devicesConnected.Add(deviceItem);
			}

			return devicesConnected.ToArray();
		}

		public async Task<bool> UploadFileAsync(string deviceId, IFormFile file)
		{
			try
			{
				StreamReader reader = new StreamReader(ConvertToBase64(file.OpenReadStream()));
				string text = reader.ReadToEnd();

				var device = await deviceRepository.QueryOneAsync(new Infrastructure.Entities.Device()
				{
					Name = deviceId
				});

				await deviceFileRepository.InsertAsync(new Infrastructure.Entities.DeviceFile()
				{
					CreateAt = DateTime.UtcNow,
					CreateBy = deviceId,
					Data = text,
					Name = deviceId,
					DeviceId = device.Id
				});

				await hubContext.Clients.All.SendAsync("UpdateDevicePicture", deviceId, text);
			}
			catch (Exception e)
			{
				logger.LogError(e, "Error on upload file.");
				return false;
			}

			return true;
		}

		public async Task<DeviceLogItemResponse[]> GetDeviceLog(Guid id)
		{
			var logs = await logRepository.QueryMultipleAsync(new DeviceLog() { DeviceId = id });

			return logs.GroupBy(g => g.RoutineKey)
					.Select(s => new DeviceLogItemResponse()
					{
						RoutineKey = s.Key,
						CreateAt = s.First().CreateAt,
						Grouped = s.Select(s => new DeviceLogGroupedItemResponse()
						{
							Id = s.Id,
							Content = s.Content,
							CreateAt = s.CreateAt,
							Type = s.Type
						}).OrderBy(o => o.CreateAt)
						.ToArray()
					})
					.Take(10)
					.ToArray();
		}

		private Stream ConvertToBase64(Stream stream)
		{
			byte[] bytes;
			using (var memoryStream = new MemoryStream())
			{
				stream.CopyTo(memoryStream);
				bytes = memoryStream.ToArray();
			}

			string base64 = Convert.ToBase64String(bytes);
			return new MemoryStream(Encoding.UTF8.GetBytes(base64));
		}
	}
}
