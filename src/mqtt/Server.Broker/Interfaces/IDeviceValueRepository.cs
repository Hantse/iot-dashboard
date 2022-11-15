using Server.Broker.Infrastructure.Entities;
using System.Threading.Tasks;
using System;

namespace Server.Broker.Interfaces
{
    public interface IDeviceValueRepository
    {
        Task<Guid?> InsertAsync(DeviceValue entity);

    }
}
