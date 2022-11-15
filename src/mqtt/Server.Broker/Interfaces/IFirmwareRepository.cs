using Server.Broker.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IFirmwareRepository
    {
        Task<IEnumerable<Firmware>> QueryMultipleAsync(Firmware query);
        Task<Guid?> InsertAsync(Firmware entity);
        Task<Firmware> QueryOneAsync(Firmware query);
    }
}
