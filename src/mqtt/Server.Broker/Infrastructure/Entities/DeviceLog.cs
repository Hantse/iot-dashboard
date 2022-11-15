using Infrastructure.Core.Persistence;
using System;

namespace Server.Broker.Infrastructure.Entities
{
    public class DeviceLog : CoreEntity
    {
        public Guid DeviceId { get; set; }
        public string Type { get; set; }
        public string RoutineKey { get; set; }
        public string Content { get; set; }
    }
}
