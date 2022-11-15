using Server.Broker.Infrastructure.Entities;
using Server.Broker.Interfaces;
using Service.Shared.Contracts.Request;
using Service.Shared.Contracts.Response;
using Service.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Broker.Business
{
    public class WorkerBusiness : IWorkerBusiness
    {
        private readonly IWorkerRepository workerRepository;

        public WorkerBusiness(IWorkerRepository workerRepository)
        {
            this.workerRepository = workerRepository;
        }

        public async Task<WorkerItemResponse> GetWorkerAsync(Guid id)
        {
            var worker = await workerRepository.QueryOneAsync(new Worker()
            {
                Id = id
            });

            if (worker == null)
                return null;

            return new WorkerItemResponse { Id = worker.Id, Name = worker.Name, Schema = JsonSerializer.Deserialize<WorkerSchema>(worker.Schema) };
        }

        public async Task<IEnumerable<WorkerItemResponse>> GetWorkersAsync()
        {
            var workers = await workerRepository.QueryMultipleAsync(new Worker()
            {
                DeleteAt = null
            });

            return workers.Select(s => new WorkerItemResponse { Id = s.Id, Name = s.Name, Schema = JsonSerializer.Deserialize<WorkerSchema>(s.Schema) }).ToArray();
        }

        public async Task<bool> SaveWorkerAsync(WorkerRequest request, Guid? id)
        {
            if (id != null)
            {
                var worker = await workerRepository.QueryOneAsync(new Worker()
                {
                    Id = (Guid)id
                });

                worker.Schema = JsonSerializer.Serialize(request.Schema);
                worker.Name = request.Name;
                worker.UpdateAt = DateTime.UtcNow;
                worker.UpdateBy = "System";

                var updateResult = await workerRepository.UpdateAsync(worker);
                return updateResult > 0;
            }
            else
            {
                var worker = new Worker()
                {
                    Id = Guid.NewGuid(),
                    Schema = JsonSerializer.Serialize(request.Schema),
                    Name = request.Name,
                    CreateAt = DateTime.UtcNow,
                    CreateBy = "System"
                };

                var insertId = await workerRepository.InsertAsync(worker);
                return insertId != null;
            }
        }
    }
}
