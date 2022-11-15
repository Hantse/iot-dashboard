using Server.Broker.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IDeviceLogRepository
    {
        Task<int> CountByTypeAsync(DeviceLog query);
        Task<Guid?> InsertAsync(DeviceLog entity);
        Task<IEnumerable<DeviceLog>> QueryMultipleAsync(DeviceLog query);
        Task<IEnumerable<DeviceLog>> QueryMultipleByTypeAsync(Guid deviceId, string type);
    }
}
