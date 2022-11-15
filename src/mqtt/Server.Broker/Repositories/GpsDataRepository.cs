using Server.Broker.Infrastructure.Entities;
using Infrastructure.Core.Persistence;
using Infrastructure.Core.Interfaces;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Server.Broker.Interfaces;
using Microsoft.Extensions.Logging;

namespace Server.Broker.Repositories
{
    public class GpsDataRepository : CoreRepository<GpsData>, IGpsDataRepository
    {
        public GpsDataRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<GpsData>> logger) 
            : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(GpsData entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeletesAsync(GpsData[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<Guid?> InsertAsync(GpsData entity)
        {
            return CoreInsertAsync(entity);
        }

        public override Task<int> InsertsAsync(GpsData[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<GpsData>> QueryMultipleAsync(GpsData query)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<GpsData>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<GpsData> QueryOneAsync(GpsData query)
        {
            throw new NotImplementedException();
        }

        public override Task<GpsData> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(GpsData entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdatesAsync(GpsData[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
