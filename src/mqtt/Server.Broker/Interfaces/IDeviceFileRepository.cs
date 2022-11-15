using Server.Broker.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IDeviceFileRepository
    {
        Task<Guid?> InsertAsync(DeviceFile entity);
        Task<IEnumerable<DeviceFile>> QueryMultipleAsync(DeviceFile query);
        Task<DeviceFile> QueryOneAsync(DeviceFile query);
    }
}
