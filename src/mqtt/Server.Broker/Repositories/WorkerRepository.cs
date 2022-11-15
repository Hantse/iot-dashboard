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
    public class WorkerRepository : CoreRepository<Worker>, IWorkerRepository
    {
        private const string SQLQUERY_SELECT_ONE = "SELECT * FROM [Worker] WHERE Id = @Id AND DeleteAt IS NULL;";
        private const string SQLQUERY_SELECT_ALL = "SELECT * FROM [Worker] WHERE DeleteAt IS NULL;";
        private const string SQLQUERY_UPDATE = "UPDATE [Worker] SET Name = @Name, [Schema] = @Schema, UpdateAt = @UpdateAt, UpdateBy = @UpdateBy WHERE Id = @Id;";

        public WorkerRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<Worker>> logger)
            : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(Worker entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeletesAsync(Worker[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<Guid?> InsertAsync(Worker entity)
        {
            return CoreInsertAsync(entity);
        }

        public override Task<int> InsertsAsync(Worker[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Worker>> QueryMultipleAsync(Worker query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryAsync<Worker>(SQLQUERY_SELECT_ALL, new { });
        }

        public override Task<IEnumerable<Worker>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<Worker> QueryOneAsync(Worker query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryFirstOrDefaultAsync<Worker>(SQLQUERY_SELECT_ONE, query);
        }

        public override Task<Worker> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(Worker entity)
        {
            return UpdateAsync(entity);
        }

        public override Task<int> UpdatesAsync(Worker[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
