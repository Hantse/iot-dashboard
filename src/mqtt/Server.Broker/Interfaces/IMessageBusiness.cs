using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IMessageBusiness
    {
        Task ProcessMessageAsync(MqttApplicationMessageReceivedEventArgs eventArgs);
    }
}
