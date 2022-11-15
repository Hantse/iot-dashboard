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
    public class DeviceRepository : CoreRepository<Device>, IDeviceRepository
    {
        private const string SQLQUERY_SELECT_ONE = "SELECT * FROM [Device] WHERE [Name] = @Name OR Id = @Id AND DeleteAt IS NULL;";
        private const string SQLQUERY_SELECT_ALL = "SELECT * FROM [Device] WHERE DeleteAt IS NULL;";

        private const string SQLQUERY_UPDATE = "UPDATE [Device] SET Ip = @Ip, BoardInformation = @BoardInformation, UpdateAt = @UpdateAt, UpdateBy = @UpdateBy WHERE Id = @Id;";

        public DeviceRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<Device>> logger)
            : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(Device entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeletesAsync(Device[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<Guid?> InsertAsync(Device entity)
        {
            return CoreInsertAsync(entity);
        }

        public override Task<int> InsertsAsync(Device[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Device>> QueryMultipleAsync(Device query)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Device>> QueryMultipleAsync()
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryAsync<Device>(SQLQUERY_SELECT_ALL, new { });
        }

        public override Task<IEnumerable<Device>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<Device> QueryOneAsync(Device query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryFirstOrDefaultAsync<Device>(SQLQUERY_SELECT_ONE, query);
        }

        public override Task<Device> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(Device entity)
        {
            return CoreUpdateAsync(SQLQUERY_UPDATE, entity);
        }

        public override Task<int> UpdatesAsync(Device[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
