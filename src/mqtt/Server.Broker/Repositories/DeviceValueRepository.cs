using Infrastructure.Core.Interfaces;
using Infrastructure.Core.Persistence;
using Microsoft.Extensions.Logging;
using Server.Broker.Infrastructure.Entities;
using Server.Broker.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Broker.Repositories
{
    public class DeviceValueRepository : CoreRepository<DeviceValue>, IDeviceValueRepository
    {
        public DeviceValueRepository(IDatabaseConnectionFactory connectionFactory, 
            ILogger<CoreRepository<DeviceValue>> logger) : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(DeviceValue entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeletesAsync(DeviceValue[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<Guid?> InsertAsync(DeviceValue entity)
        {
            return CoreInsertAsync(entity);
        }

        public override Task<int> InsertsAsync(DeviceValue[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<DeviceValue>> QueryMultipleAsync(DeviceValue query)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<DeviceValue>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<DeviceValue> QueryOneAsync(DeviceValue query)
        {
            throw new NotImplementedException();
        }

        public override Task<DeviceValue> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(DeviceValue entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdatesAsync(DeviceValue[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
