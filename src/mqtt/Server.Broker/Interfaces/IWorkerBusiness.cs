using Service.Shared.Contracts.Request;
using Service.Shared.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Broker.Interfaces
{
    public interface IWorkerBusiness
    {
        Task<IEnumerable<WorkerItemResponse>> GetWorkersAsync();
        Task<WorkerItemResponse> GetWorkerAsync(Guid id);
        Task<bool> SaveWorkerAsync(WorkerRequest request, Guid? id);
    }
}
