using Microsoft.AspNetCore.Mvc;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Broker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MqttServerClientController : ControllerBase
    {
        private readonly IMqttServer mqttServer;

        public MqttServerClientController(IMqttServer mqttServer)
        {
            this.mqttServer = mqttServer;
        }

        [HttpGet]
        [Route("ServerStatus")]
        public IActionResult ServerStatus()
        {
            return Ok(mqttServer.IsStarted);
        }

        [HttpGet]
        [Route("update")]
        public async Task<IActionResult> Update()
        {
           
            return Ok();
        }

        public async Task<IActionResult> Get()
        {
            return Ok(mqttServer.GetClientStatusAsync());
        }
    }
}
