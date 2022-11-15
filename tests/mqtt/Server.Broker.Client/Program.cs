using MQTTnet;
using MQTTnet.Client.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Broker.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                                .WithTcpServer("localhost", 1883)
                                .Build();

            await mqttClient.ConnectAsync(options, CancellationToken.None);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic("mqttweatherforecast/90210/temperature")
                .WithPayload("25")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await mqttClient.PublishAsync(message, CancellationToken.None);

        }
    }
}
