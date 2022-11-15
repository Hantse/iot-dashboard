using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client.Receiving;
using Server.Broker.Interfaces;
using System.Threading.Tasks;

namespace Server.Broker.Infrastructure.Handlers
{
    public class OnMessageHandler : IMqttApplicationMessageReceivedHandler
    {
        private readonly ILogger<OnMessageHandler> logger;
        private readonly IMessageBusiness messageBusiness;

        public OnMessageHandler(ILogger<OnMessageHandler> logger, IMessageBusiness messageBusiness)
        {
            this.logger = logger;
            this.messageBusiness = messageBusiness;
        }

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            await messageBusiness.ProcessMessageAsync(eventArgs);
            logger.LogInformation($"Message receive - {eventArgs.ApplicationMessage.Topic}.");
        }
    }
}
