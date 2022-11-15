using Infrastructure.Core.Persistence;
using System;

namespace Server.Broker.Infrastructure.Entities
{
    public class DeviceFile : CoreEntity
    {
        public string Name { get; set; }
        public Guid DeviceId { get; set; }
        public string Data { get; set; }
    }
}
