using Server.Broker.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IWorkerRepository
    {
        Task<Guid?> InsertAsync(Worker entity);
        Task<IEnumerable<Worker>> QueryMultipleAsync(Worker query);
        Task<Worker> QueryOneAsync(Worker query);
        Task<int> UpdateAsync(Worker entity);
    }
}
