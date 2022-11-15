using Dapper;
using Infrastructure.Core.Interfaces;
using Infrastructure.Core.Persistence;
using Microsoft.Extensions.Logging;
using Server.Broker.Infrastructure.Entities;
using Server.Broker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Broker.Repositories
{
    public class DeviceLogRepository : CoreRepository<DeviceLog>, IDeviceLogRepository
    {
        private const string SQLQUERY_SELECT_ALL = "SELECT * FROM DeviceLog WHERE DeviceId = @DeviceId ORDER BY CreateAt DESC";
        private const string SQLQUERY_SELECT_LASTEVENT = "SELECT * FROM DeviceLog WHERE DeviceId = @DeviceId AND [Type] = @Type";
        private const string SQLQUERY_SELECT_COUNT_BY_TYPE = "SELECT COUNT(*) FROM DeviceLog WHERE DeviceId = @DeviceId AND [Type] = @Type";

        public DeviceLogRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<DeviceLog>> logger)
            : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(DeviceLog entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeletesAsync(DeviceLog[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<Guid?> InsertAsync(DeviceLog entity)
        {
            return CoreInsertAsync(entity);
        }

        public override Task<int> InsertsAsync(DeviceLog[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeviceLog>> QueryMultipleByTypeAsync(Guid deviceId, string type)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryAsync<DeviceLog>(SQLQUERY_SELECT_LASTEVENT, new { DeviceId = deviceId, Type = type });
        }


        public override Task<IEnumerable<DeviceLog>> QueryMultipleAsync(DeviceLog query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryAsync<DeviceLog>(SQLQUERY_SELECT_ALL, query);
        }

        public override Task<IEnumerable<DeviceLog>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }
        public Task<int> CountByTypeAsync(DeviceLog query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryFirstAsync<int>(SQLQUERY_SELECT_COUNT_BY_TYPE, query);
        }

        public override Task<DeviceLog> QueryOneAsync(DeviceLog query)
        {
            throw new NotImplementedException();
        }

        public override Task<DeviceLog> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(DeviceLog entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdatesAsync(DeviceLog[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
