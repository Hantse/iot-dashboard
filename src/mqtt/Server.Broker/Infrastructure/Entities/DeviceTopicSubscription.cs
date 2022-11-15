using Infrastructure.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Broker.Infrastructure.Entities
{
    public class DeviceTopicSubscription : CoreEntity
    {
        public string Name { get; set; }
        public Guid DeviceId { get; set; }
        public string Topic { get; set; }
    }
}
