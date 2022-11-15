using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using Server.Broker.Hubs;
using Server.Broker.Infrastructure.Entities;
using Server.Broker.Interfaces;
using Server.Broker.Repositories;
using Server.Broker.Services;
using Service.Shared.Contracts.Response;
using System;
using System.Threading.Tasks;

namespace Server.Broker.Infrastructure.Handlers
{
    public class OnClientConnectedHandler : IMqttServerClientConnectedHandler
    {
        private readonly IHubContext<DeviceHub> hubContext;
        private readonly ILogger<OnClientConnectedHandler> logger;
        private readonly IDeviceLogService deviceLogService;
        private readonly IDeviceRepository deviceRepository;
        private readonly IMessageRepository messageRepository;
        private readonly IDeviceLogRepository logRepository;
        private readonly IDeviceFileRepository deviceFileRepository;


        public OnClientConnectedHandler(IHubContext<DeviceHub> hubContext, ILogger<OnClientConnectedHandler> logger, IDeviceLogService deviceLogService, IDeviceRepository deviceRepository, IMessageRepository messageRepository, IDeviceLogRepository logRepository, IDeviceFileRepository deviceFileRepository)
        {
            this.hubContext = hubContext;
            this.logger = logger;
            this.deviceLogService = deviceLogService;
            this.deviceRepository = deviceRepository;
            this.messageRepository = messageRepository;
            this.logRepository = logRepository;
            this.deviceFileRepository = deviceFileRepository;
        }

        public async Task HandleClientConnectedAsync(MqttServerClientConnectedEventArgs eventArgs)
        {
            logger.LogInformation($"Client connected {eventArgs.ClientId}.");

            var device = await deviceRepository.QueryOneAsync(new Device() { Name = eventArgs.ClientId });

            if (device == null)
            {
                device = new Device()
                {
                    Name = eventArgs.ClientId,
                    CreateAt = DateTime.UtcNow,
                    CreateBy = eventArgs.ClientId,
                    Id = Guid.NewGuid(),
                    Status = "CONNECTED",
                    LastPing = DateTime.UtcNow
                };

                if (await deviceRepository.InsertAsync(device) != null)
                {
                    await hubContext.Clients.All.SendAsync("UpdateDeviceListState", eventArgs.ClientId, new DeviceItemResponse()
                    {
                        BoardInformation = device.BoardInformation,
                        Id = device.Id,
                        Online = true,
                        ConnectionRoutineCount = 1,
                        PingRoutineCount = 1,
                        SubscribeRoutineCount = 1,
                        UnsubscribeRoutineCount = 1,
                        MessageRoutineCount = 1
                    });

                    logger.LogInformation($"New device insert {eventArgs.ClientId}.");
                }
            }
            else
            {
                await hubContext.Clients.All.SendAsync("UpdateDeviceState", eventArgs.ClientId, true);
            }

            await deviceLogService.LogConnection(eventArgs.ClientId);
        }
    }
}
