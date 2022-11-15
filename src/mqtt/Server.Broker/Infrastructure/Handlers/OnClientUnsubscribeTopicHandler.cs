using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using Server.Broker.Hubs;
using Server.Broker.Interfaces;
using System;
using System.Threading.Tasks;

namespace Server.Broker.Infrastructure.Handlers
{
    public class OnClientUnsubscribeTopicHandler : IMqttServerClientUnsubscribedTopicHandler
    {
        private readonly IHubContext<DeviceHub> hubContext;
        private readonly ILogger<OnClientConnectedHandler> logger;
        private readonly IDeviceLogService deviceLogService;
        private readonly IDeviceRepository deviceRepository;
        private readonly IDeviceTopicSubscriptionRepository deviceTopicSubscriptionRepository;

        public OnClientUnsubscribeTopicHandler(IHubContext<DeviceHub> hubContext, ILogger<OnClientConnectedHandler> logger,
            IDeviceLogService deviceLogService, IDeviceRepository deviceRepository, IDeviceTopicSubscriptionRepository deviceTopicSubscriptionRepository)
        {
            this.hubContext = hubContext;
            this.logger = logger;
            this.deviceLogService = deviceLogService;
            this.deviceRepository = deviceRepository;
            this.deviceTopicSubscriptionRepository = deviceTopicSubscriptionRepository;
        }

        public async Task HandleClientUnsubscribedTopicAsync(MqttServerClientUnsubscribedTopicEventArgs eventArgs)
        {
            logger.LogInformation($"Client {eventArgs.ClientId} unsubscribe to {eventArgs.TopicFilter}.");
            await deviceLogService.LogUnsubscribe(eventArgs.ClientId, eventArgs.TopicFilter);

            var device = await deviceRepository.QueryOneAsync(new Entities.Device() { Name = eventArgs.ClientId });

            await deviceTopicSubscriptionRepository.DeleteAsync(new Entities.DeviceTopicSubscription()
            {
                Topic = eventArgs.TopicFilter,
                DeviceId = device.Id,
                DeleteAt = DateTime.UtcNow,
                DeleteBy = eventArgs.ClientId
            });
        }
    }
}
