using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using Server.Broker.Hubs;
using Server.Broker.Infrastructure.Entities;
using Server.Broker.Interfaces;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Broker.Infrastructure.Handlers
{
    public class OnClientSubscribeTopicHandler : IMqttServerClientSubscribedTopicHandler
    {
        private readonly IMqttServer mqttServer;

        private readonly IHubContext<DeviceHub> hubContext;
        private readonly ILogger<OnClientConnectedHandler> logger;
        private readonly IDeviceLogService deviceLogService;
        private readonly IDeviceRepository deviceRepository;
        private readonly IDeviceTopicSubscriptionRepository deviceTopicSubscriptionRepository;
        private readonly IMessageRepository messageRepository;

        public OnClientSubscribeTopicHandler(IHubContext<DeviceHub> hubContext, ILogger<OnClientConnectedHandler> logger, IDeviceLogService deviceLogService, IDeviceRepository deviceRepository, IDeviceTopicSubscriptionRepository deviceTopicSubscriptionRepository, IMessageRepository messageRepository, IMqttServer mqttServer)
        {
            this.hubContext = hubContext;
            this.logger = logger;
            this.deviceLogService = deviceLogService;
            this.deviceRepository = deviceRepository;
            this.deviceTopicSubscriptionRepository = deviceTopicSubscriptionRepository;
            this.messageRepository = messageRepository;
            this.mqttServer = mqttServer;
        }

        public async Task HandleClientSubscribedTopicAsync(MqttServerClientSubscribedTopicEventArgs eventArgs)
        {
            logger.LogInformation($"Client {eventArgs.ClientId} subscribe to {eventArgs.TopicFilter.Topic}.");
            await deviceLogService.LogSubscribe(eventArgs.ClientId, eventArgs.TopicFilter.Topic);

            var device = await deviceRepository.QueryOneAsync(new Entities.Device() { Name = eventArgs.ClientId });
            var deviceSubscriptions = await deviceTopicSubscriptionRepository.QueryMultipleAsync(new Entities.DeviceTopicSubscription()
            {
                DeviceId = device.Id
            });

            var subscriptionTopics = deviceSubscriptions.Select(s => s.Topic).ToArray();

            if (!deviceSubscriptions.Any(ds => ds.Topic == eventArgs.TopicFilter.Topic))
            {
                await deviceTopicSubscriptionRepository.InsertAsync(new DeviceTopicSubscription()
                {
                    CreateAt = System.DateTime.UtcNow,
                    CreateBy = eventArgs.ClientId,
                    DeviceId = device.Id,
                    Name = device.Name,
                    Topic = eventArgs.TopicFilter.Topic
                });

                subscriptionTopics.Append(eventArgs.TopicFilter.Topic);
            }

            await hubContext.Clients.All.SendAsync("UpdateDeviceSubscriptionState", eventArgs.ClientId, subscriptionTopics);

            var messages = await messageRepository.QueryMultipleAsync(new Message()
            {
                DeviceName = eventArgs.ClientId,
                Topic = eventArgs.TopicFilter.Topic
            });

            foreach (var message in messages)
            {
                await mqttServer.PublishAsync(new MQTTnet.MqttApplicationMessage()
                {
                    Topic = message.Topic,
                    Payload = Encoding.UTF8.GetBytes(message.Content)
                });

                message.UpdateAt = System.DateTime.UtcNow;
                message.UpdateBy = "System On Subscribe";
                message.DeliverAt = System.DateTime.UtcNow;
            }

            await messageRepository.UpdatesAsync(messages.ToArray());
        }
    }
}
