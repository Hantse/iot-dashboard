using Server.Broker.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IFirmwareVersionRepository
    {
        Task<FirmwareVersion> QueryOneAsync(FirmwareVersion query);
        Task<Guid?> InsertAsync(FirmwareVersion entity);
        Task<IEnumerable<FirmwareVersion>> QueryMultipleAsync(FirmwareVersion query);
    }
}
