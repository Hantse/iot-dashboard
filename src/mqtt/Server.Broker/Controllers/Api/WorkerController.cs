using Microsoft.AspNetCore.Mvc;
using Server.Broker.Interfaces;
using Service.Shared.Contracts.Request;
using System;
using System.Threading.Tasks;

namespace Server.Broker.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerBusiness workerBusiness;

        public WorkerController(IWorkerBusiness workerBusiness)
        {
            this.workerBusiness = workerBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await workerBusiness.GetWorkersAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await workerBusiness.GetWorkerAsync(id));
        }

        [HttpPost("{id?}")]
        public async Task<IActionResult> SaveWorkerAsync(Guid? id, WorkerRequest request)
        {
            var saveResult = await workerBusiness.SaveWorkerAsync(request, id);

            if (saveResult)
                return Ok();

            return BadRequest();
        }
    }
}
