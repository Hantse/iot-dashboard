using Microsoft.AspNetCore.Mvc;
using Server.Broker.Interfaces;
using Service.Shared.Contracts.Request;
using System;
using System.Threading.Tasks;

namespace Server.Broker.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class FirmwareController : ControllerBase
    {
        private readonly IFirmwareBusiness firmwareBusiness;

        public FirmwareController(IFirmwareBusiness firmwareBusiness)
        {
            this.firmwareBusiness = firmwareBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> GetFirmwares()
        {
            return Ok(await firmwareBusiness.GetFirmwaresAsync());
        }

        [HttpGet("{id}/versions")]
        public async Task<IActionResult> GetFirmwareVersions([FromRoute]Guid id)
        {
            return Ok(await firmwareBusiness.GetFirmwareVersionsAsync(id));
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFirmware()
        {
            if (this.Request.Form.Files == null || this.Request.Form.Files.Count == 0)
                return BadRequest();

            var file = Request.Form.Files[0];

            var updateResult = await firmwareBusiness.UploadFileAsync(file);

            if (updateResult.success)
                return Ok();

            if (!updateResult.success && updateResult.isConflict)
                return Conflict();

            return BadRequest();
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> DownloadFirmware([FromRoute] string id)
        {
            var binId = id.Replace(".bin", "");

            var version = await firmwareBusiness.GetFirmwareVersionAsync(Guid.Parse(binId));

            if (version == null)
                return NotFound();

            return File(version?.Content, "application/octet-stream", $"{id}");
        }

        [HttpPost("install/{id}")]
        public async Task<IActionResult> InstallFirmware([FromBody] InstallOnDeviceRequest request, [FromRoute] Guid id)
        {
            foreach (var device in request.Devices)
            {
                await firmwareBusiness.InstallOnDeviceAsync(device, id);
            }

            return Ok();
        }

        [HttpGet("install/{deviceName}/{id}")]
        public async Task<IActionResult> InstallFirmware([FromRoute] string deviceName, [FromRoute] Guid id)
        {
            if (await firmwareBusiness.InstallOnDeviceAsync(deviceName, id))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
