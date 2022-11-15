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
    public class FirmwareVersionRepository : CoreRepository<FirmwareVersion>, IFirmwareVersionRepository
    {
        private const string SQLQUERY_SELECT_ONE = "SELECT * FROM [FirmwareVersion] WHERE (Checksum = @Checksum OR Id = @Id) AND DeleteAt IS NULL;";
        private const string SQLQUERY_SELECT_ALL = "SELECT Id, CreateAt, Version, Checksum FROM [FirmwareVersion] WHERE FirmwareId = @FirmwareId AND DeleteAt IS NULL;";

        public FirmwareVersionRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<FirmwareVersion>> logger) : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(FirmwareVersion entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeletesAsync(FirmwareVersion[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<Guid?> InsertAsync(FirmwareVersion entity)
        {
            return CoreInsertAsync(entity);
        }

        public override Task<int> InsertsAsync(FirmwareVersion[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<FirmwareVersion>> QueryMultipleAsync(FirmwareVersion query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryAsync<FirmwareVersion>(SQLQUERY_SELECT_ALL, query);
        }

        public override Task<IEnumerable<FirmwareVersion>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<FirmwareVersion> QueryOneAsync(FirmwareVersion query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryFirstOrDefaultAsync<FirmwareVersion>(SQLQUERY_SELECT_ONE, query);
        }

        public override Task<FirmwareVersion> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(FirmwareVersion entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdatesAsync(FirmwareVersion[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
