using Infrastructure.Core.Persistence;
using System;

namespace Server.Broker.Infrastructure.Entities
{
    public class Message : CoreEntity
    {
        public string DeviceName { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public DateTime? DeliverAt { get; set; }
    }
}
