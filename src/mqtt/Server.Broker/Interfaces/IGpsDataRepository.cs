using System.Threading.Tasks;
using System;
using Server.Broker.Infrastructure.Entities;

namespace Server.Broker.Interfaces
{
    public interface IGpsDataRepository
    {
        Task<Guid?> InsertAsync(GpsData entity);
    }
}
