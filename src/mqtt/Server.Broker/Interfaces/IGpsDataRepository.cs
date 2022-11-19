using System.Threading.Tasks;
using System;
using Server.Broker.Infrastructure.Entities;

namespace Server.Broker.Interfaces
{
    public interface IGpsDataRepository
    {
        Task<GpsData> QueryLastOneAsync(GpsData query);
        Task<System.Collections.Generic.IEnumerable<GpsData>> QueryMultipleAsync(GpsData query);
		Task<Guid?> InsertAsync(GpsData entity);

    }
}
