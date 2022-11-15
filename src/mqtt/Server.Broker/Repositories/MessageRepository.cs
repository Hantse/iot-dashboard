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
    public class MessageRepository : CoreRepository<Message>, IMessageRepository
    {
        private const string SQLQUERY_SELECT_DEVICE_PENDING = "SELECT * FROM [Message] WHERE DeviceName = @DeviceName AND [Topic] = @Topic AND DeliverAt IS NULL AND DeleteAt IS NULL;";
        private const string SQLQUERY_UPDATE = "UPDATE [Message] SET DeliverAt = @DeliverAt, UpdateAt = @UpdateAt, UpdateBy = @UpdateBy WHERE Id = @Id;";

        public MessageRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<Message>> logger) : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(Message entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> DeletesAsync(Message[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<Guid?> InsertAsync(Message entity)
        {
            return CoreInsertAsync(entity);
        }

        public override Task<int> InsertsAsync(Message[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<Message>> QueryMultipleAsync(Message query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryAsync<Message>(SQLQUERY_SELECT_DEVICE_PENDING, query);
        }

        public override Task<IEnumerable<Message>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<Message> QueryOneAsync(Message query)
        {
            throw new NotImplementedException();
        }

        public override Task<Message> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(Message entity)
        {
            return UpdateAsync(entity);
        }

        public override Task<int> UpdatesAsync(Message[] entities)
        {
            return CoreUpdatesAsync(SQLQUERY_UPDATE, entities);
        }
    }
}
