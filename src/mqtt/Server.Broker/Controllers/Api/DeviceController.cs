using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MQTTnet.Server;
using Server.Broker.Hubs;
using Server.Broker.Interfaces;
using Service.Shared.Contracts.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Broker.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceBusiness deviceBusiness;
        private readonly IMqttServer mqttServer;
        private readonly IHubContext<DeviceHub> hubContext;
        private readonly IDeviceLogService deviceLogService;

        public DeviceController(IDeviceBusiness deviceBusiness, IMqttServer mqttServer, IHubContext<DeviceHub> hubContext, IDeviceLogService deviceLogService)
        {
            this.deviceBusiness = deviceBusiness;
            this.mqttServer = mqttServer;
            this.hubContext = hubContext;
            this.deviceLogService = deviceLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await deviceBusiness.GetDevicesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDevice(Guid id)
        {
            var devices = await deviceBusiness.GetDevicesAsync();
            var device = devices.FirstOrDefault(d => d.Id == id);
            if (device == null)
                return NotFound();

            return Ok(device);
        }

        [HttpGet("{id}/logs")]
        public async Task<IActionResult> GetDeviceLogs(Guid id)
        {
            var logs = await deviceBusiness.GetDeviceLog(id);
            return Ok(logs);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessageAsync([FromBody] DeviceMessageRequest message)
        {
            await mqttServer.PublishAsync(new MQTTnet.MqttApplicationMessage()
            {
                Topic = message.Topic,
                Payload = Encoding.UTF8.GetBytes(message.Message),
                QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.ExactlyOnce
            });
            return Ok();
        }

        [HttpPost("{deviceId}/picture"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadPicture([FromRoute] string deviceId, List<IFormFile> files)
        {
            await deviceLogService.LogFile(deviceId);
            if (await deviceBusiness.UploadFileAsync(deviceId, Request.Form.Files[0]))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
