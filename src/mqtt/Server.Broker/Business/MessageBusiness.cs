using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Server;
using Server.Broker.Infrastructure.Entities;
using Server.Broker.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Broker.Business
{
    public class MessageBusiness : IMessageBusiness
    {
        private readonly IMessageRepository messageRepository;
        private readonly IDeviceTopicSubscriptionRepository deviceTopicSubscriptionRepository;
        private readonly IDeviceRepository deviceRepository;
        private readonly ILogger<MessageBusiness> logger;
        private readonly IMqttServer mqttServer;

        public MessageBusiness(IMessageRepository messageRepository, ILogger<MessageBusiness> logger, IMqttServer mqttServer, IDeviceTopicSubscriptionRepository deviceTopicSubscriptionRepository, IDeviceRepository deviceRepository)
        {
            this.messageRepository = messageRepository;
            this.logger = logger;
            this.mqttServer = mqttServer;
            this.deviceTopicSubscriptionRepository = deviceTopicSubscriptionRepository;
            this.deviceRepository = deviceRepository;
        }

        public async Task ProcessMessageAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            var content = eventArgs.ApplicationMessage.ConvertPayloadToString();
            var subscriptions = await deviceTopicSubscriptionRepository.QueryMultipleByTopicAsync(new DeviceTopicSubscription()
            {
                Topic = eventArgs.ApplicationMessage.Topic
            });
            var devices = await mqttServer.GetClientStatusAsync();

            foreach (var subscription in subscriptions)
            {
                var device = await deviceRepository.QueryOneAsync(new Device()
                {
                    Id = subscription.DeviceId
                });

                if (!devices.Any(d => d.ClientId == device.Name))
                {
                    await messageRepository.InsertAsync(new Message()
                    {
                        Content = content,
                        Topic = eventArgs.ApplicationMessage.Topic,
                        DeviceName = device.Name,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = "System",
                        Id = Guid.NewGuid()
                    });
                }
                else
                {
                    await messageRepository.InsertAsync(new Message()
                    {
                        Content = content,
                        Topic = eventArgs.ApplicationMessage.Topic,
                        DeviceName = device.Name,
                        CreateAt = DateTime.UtcNow,
                        CreateBy = "System",
                        Id = Guid.NewGuid(),
                        DeliverAt = DateTime.UtcNow
                    });
                }
            }
        }
    }
}
