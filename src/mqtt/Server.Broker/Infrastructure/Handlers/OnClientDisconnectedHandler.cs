using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MQTTnet.Server;
using Server.Broker.Hubs;
using Server.Broker.Interfaces;
using System.Threading.Tasks;

namespace Server.Broker.Infrastructure.Handlers
{
    public class OnClientDisconnectedHandler : IMqttServerClientDisconnectedHandler
    {
        private readonly IHubContext<DeviceHub> hubContext;
        private readonly ILogger<OnClientDisconnectedHandler> logger;
        private readonly IDeviceLogService deviceLogService;

        public OnClientDisconnectedHandler(IHubContext<DeviceHub> hubContext, ILogger<OnClientDisconnectedHandler> logger, IDeviceLogService deviceLogService)
        {
            this.hubContext = hubContext;
            this.logger = logger;
            this.deviceLogService = deviceLogService;
        }

        public async Task HandleClientDisconnectedAsync(MqttServerClientDisconnectedEventArgs eventArgs)
        {
            logger.LogInformation($"Client disconnected {eventArgs.ClientId}.");
            await deviceLogService.LogDisconnection(eventArgs.ClientId);
            await hubContext.Clients.All.SendAsync("UpdateDeviceState", eventArgs.ClientId, false);
        }
    }
}
