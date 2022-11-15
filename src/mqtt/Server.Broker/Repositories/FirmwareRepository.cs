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
    public class FirmwareRepository : CoreRepository<Firmware>, IFirmwareRepository
    {

        private const string SQLQUERY_SELECT_ONE = "SELECT * FROM [Firmware] WHERE (Name = @Name OR Id = @Id) AND DeleteAt IS NULL;";
        private const string SQLQUERY_SELECT_ALL = "SELECT * FROM [Firmware] WHERE DeleteAt IS NULL;";

        public FirmwareRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<Firmware>> logger) : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(Firmware entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeletesAsync(Firmware[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<Guid?> InsertAsync(Firmware entity)
        {
            return CoreInsertAsync(entity);
        }

        public override Task<int> InsertsAsync(Firmware[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Firmware>> QueryMultipleAsync(Firmware query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryAsync<Firmware>(SQLQUERY_SELECT_ALL, new { });
        }

        public override Task<IEnumerable<Firmware>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<Firmware> QueryOneAsync(Firmware query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryFirstOrDefaultAsync<Firmware>(SQLQUERY_SELECT_ONE, query);
        }

        public override Task<Firmware> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(Firmware entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdatesAsync(Firmware[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
