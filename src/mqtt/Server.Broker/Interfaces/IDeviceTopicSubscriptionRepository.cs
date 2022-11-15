using Server.Broker.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IDeviceTopicSubscriptionRepository
    {
        Task<int> DeleteAsync(DeviceTopicSubscription entity);
        Task<Guid?> InsertAsync(DeviceTopicSubscription entity);
        Task<IEnumerable<DeviceTopicSubscription>> QueryMultipleByTopicAsync(DeviceTopicSubscription query);
        Task<IEnumerable<DeviceTopicSubscription>> QueryMultipleAsync(DeviceTopicSubscription query);
    }
}
