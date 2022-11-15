using Infrastructure.Core.Persistence;

namespace Server.Broker.Infrastructure.Entities
{
    public class Worker : CoreEntity
    {
        public string Name { get; set; }
        public string Schema { get; set; }
    }
}
