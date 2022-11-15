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
    public class DeviceTopicSubscriptionRepository : CoreRepository<DeviceTopicSubscription>, IDeviceTopicSubscriptionRepository
    {
        private const string SQLQUERY_SELECT_ALL_FROM_DEVICE = "SELECT * FROM [DeviceTopicSubscription] WHERE DeviceId = @DeviceId AND DeleteAt IS NULL;";
        private const string SQLQUERY_DELETE_ALL_FROM_DEVICE = "UPDATE DeviceTopicSubscription SET DeleteAt = @DeleteAt, DeleteBy = @DeleteBy " +
                                                                 "WHERE DeviceId = @DeviceId AND [Topic] = @Topic AND DeleteAt IS NULL; ";

        private const string SQLQUERY_SELECT_ALL_DEVICE_FROM_TOPIC = "SELECT * FROM [DeviceTopicSubscription] WHERE [Topic] = @Topic AND DeleteAt IS NULL;";

        public DeviceTopicSubscriptionRepository(IDatabaseConnectionFactory connectionFactory, ILogger<CoreRepository<DeviceTopicSubscription>> logger)
            : base(connectionFactory, logger)
        {
        }

        public override Task<int> DeleteAsync(DeviceTopicSubscription entity)
        {
            return CoreDeleteAsync(SQLQUERY_DELETE_ALL_FROM_DEVICE, entity);
        }

        public override Task<int> DeletesAsync(DeviceTopicSubscription[] entities)
        {
            throw new NotImplementedException();
        }

        public override Task<Guid?> InsertAsync(DeviceTopicSubscription entity)
        {
            return CoreInsertAsync(entity);
        }

        public override Task<int> InsertsAsync(DeviceTopicSubscription[] entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DeviceTopicSubscription>> QueryMultipleByTopicAsync(DeviceTopicSubscription query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryAsync<DeviceTopicSubscription>(SQLQUERY_SELECT_ALL_DEVICE_FROM_TOPIC, query);
        }

        public override Task<IEnumerable<DeviceTopicSubscription>> QueryMultipleAsync(DeviceTopicSubscription query)
        {
            var connection = connectionFactory.GetConnection();
            return connection.QueryAsync<DeviceTopicSubscription>(SQLQUERY_SELECT_ALL_FROM_DEVICE, query);
        }

        public override Task<IEnumerable<DeviceTopicSubscription>> QueryMultipleByIdAsync(Guid[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<DeviceTopicSubscription> QueryOneAsync(DeviceTopicSubscription query)
        {
            throw new NotImplementedException();
        }

        public override Task<DeviceTopicSubscription> QueryOneByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdateAsync(DeviceTopicSubscription entity)
        {
            throw new NotImplementedException();
        }

        public override Task<int> UpdatesAsync(DeviceTopicSubscription[] entities)
        {
            throw new NotImplementedException();
        }
    }
}
