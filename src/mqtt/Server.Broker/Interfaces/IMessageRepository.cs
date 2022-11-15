using Server.Broker.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> QueryMultipleAsync(Message query);
        Task<Guid?> InsertAsync(Message entity);
        Task<int> UpdateAsync(Message entity);
        Task<int> UpdatesAsync(Message[] entities);
    }
}
