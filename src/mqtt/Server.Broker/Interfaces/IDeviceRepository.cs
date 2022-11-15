using Server.Broker.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IDeviceRepository
    {
        Task<Guid?> InsertAsync(Device entity);
        Task<Device> QueryOneAsync(Device query);
        Task<IEnumerable<Device>> QueryMultipleAsync();
        Task<int> UpdateAsync(Device entity);
    }
}
