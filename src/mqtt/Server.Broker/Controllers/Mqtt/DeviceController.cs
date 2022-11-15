using Microsoft.Extensions.Logging;
using MQTTnet.AspNetCore.AttributeRouting;
using Server.Broker.Interfaces;
using System;
using System.Threading.Tasks;

namespace Server.Broker.Controllers.Mqtt
{
    [MqttController]
    [MqttRoute("mqtt/[controller]")]
    public class DeviceController : MqttBaseController
    {
        private readonly ILogger<DeviceController> logger;
        private readonly IDeviceLogService deviceLogService;
        private readonly IDeviceBusiness deviceBusiness;
        private readonly IDeviceValueBusiness deviceValueBusiness;

        public DeviceController(ILogger<DeviceController> logger, IDeviceLogService deviceLogService, IDeviceBusiness deviceBusiness, IDeviceValueBusiness deviceValueBusiness)
        {
            this.logger = logger;
            this.deviceLogService = deviceLogService;
            this.deviceBusiness = deviceBusiness;
            this.deviceValueBusiness = deviceValueBusiness;
        }

        [MqttRoute("{name}/update-device")]
        public Task UpdateDevice(string name)
        {
            logger.LogInformation($"Device Updated {name}.");
            logger.LogInformation($"Device Updated {GetPayloadIfExist()}.");
            return Ok();
        }

        [MqttRoute("{name}/informations")]
        public Task UpdateDeviceInformations(string name)
        {
            var ip = GetPayloadIfExist();
            logger.LogInformation($"Device IP {ip}");

            return Ok();
        }

        [MqttRoute("{name}/ping")]
        public async Task PingDeviceInformations(string name)
        {
            logger.LogInformation($"Ping from {name}.");
            await deviceLogService.LogPing(name);
        }

        [MqttRoute("{name}/data/{type}")]
        public async Task DataDevice(string name, string type)
        {
            if (type.Equals("board", StringComparison.OrdinalIgnoreCase))
            {
                await deviceBusiness.UpdateDeviceBoardInformationsAsync(name, GetPayloadIfExist());
            }
            else if (type.Equals("telemetry", StringComparison.OrdinalIgnoreCase))
            {
                await deviceValueBusiness.InsertDeviceValue(Guid.NewGuid().ToString("N"), name, GetPayloadIfExist());
            }
            else if (type.Equals("gps", StringComparison.OrdinalIgnoreCase))
            {
                await deviceValueBusiness.InsertDeviceGpsValue(Guid.NewGuid().ToString("N"), name, GetPayloadIfExist());
            }

            logger.LogInformation($"Data from {name} - {type} - {GetPayloadIfExist()}.");
        }

        [MqttRoute("{name}/data/{type}/{value}")]
        public async Task PatchDeviceInfo(string name, string type, string value)
        {
            if (type.Equals("ip", StringComparison.OrdinalIgnoreCase))
            {
                await deviceBusiness.UpdateDeviceIpAsync(name, value);
            }

            logger.LogInformation($"Data from {name} - {type} - {value}.");
        }

        private string GetPayloadIfExist()
        {
            if (Message != null && Message.Payload != null)
                return System.Text.Encoding.UTF8.GetString(Message?.Payload);

            return string.Empty;
        }
    }
}
