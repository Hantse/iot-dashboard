using Dapper;
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
    public class DeviceFileRepository : CoreRepository<DeviceFile>, IDeviceFileRepository
    {
        private const string SQLQUERY_SELECT_ONE = "SELECT TOP 1 * FROM DeviceFile WHERE [Name] = @Name AND DeleteAt IS NULL ORDER BY CreateAt DESC;";
        private const string SQLQUERY_SELECT_ALL = "SELECT * FROM [DeviceFile] WHERE [Name] = @Name AND DeleteAt IS NULL ORDER BY CreateAt DESC;";

        public DeviceFileRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<DeviceFile>> logger)
            : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(DeviceFile entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeletesAsync(DeviceFile[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<Guid?> InsertAsync(DeviceFile entity)
        {
            return CoreInsertAsync(entity);
        }

        public override Task<int> InsertsAsync(DeviceFile[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<DeviceFile>> QueryMultipleAsync(DeviceFile query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryAsync<DeviceFile>(SQLQUERY_SELECT_ALL, query);
        }

        public override Task<IEnumerable<DeviceFile>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<DeviceFile> QueryOneAsync(DeviceFile query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryFirstOrDefaultAsync<DeviceFile>(SQLQUERY_SELECT_ONE, query);
        }

        public override Task<DeviceFile> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(DeviceFile entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdatesAsync(DeviceFile[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
