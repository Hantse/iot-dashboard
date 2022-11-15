using Infrastructure.Core.Persistence;
using System;

namespace Server.Broker.Infrastructure.Entities
{
    public class DeviceValue : CoreEntity
    {
        public Guid DeviceId { get; set; }
        public string CorrelationId { get; set; }
        public string DeviceName { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public DateTime? DeliverAt { get; set; }

    }
}
