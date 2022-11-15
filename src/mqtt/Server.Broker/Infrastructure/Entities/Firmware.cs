using Infrastructure.Core.Persistence;

namespace Server.Broker.Infrastructure.Entities
{
    public class Firmware : CoreEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
