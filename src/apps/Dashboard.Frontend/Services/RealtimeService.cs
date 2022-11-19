using AntDesign;
using Dashboard.Frontend.Interfaces;
using Dashboard.Frontend.Stores.Actions.Device;
using Fluxor;
using Microsoft.AspNetCore.SignalR.Client;
using Service.Shared.Contracts.Response;
using System;
using System.Threading.Tasks;
using static Dashboard.Frontend.Stores.Actions.Firmware.FirmwareAction;

namespace Dashboard.Frontend.Services
{
	public class RealtimeService : IRealtimeService
	{
		private HubConnection hubConnection;

		private IDispatcher dispatcher;

		private NotificationService notificationService;


		public RealtimeService(IDispatcher dispatcher, NotificationService notificationService)
		{
			this.notificationService = notificationService;
			this.dispatcher = dispatcher;

			hubConnection = new HubConnectionBuilder()
						   .WithUrl("http://localhost:5001/device")
						   .WithAutomaticReconnect()
						   .Build();

			hubConnection.On<string, bool>("UpdateDeviceState", async (deviceId, state) =>
			{
				if (state)
				{
					await this.notificationService.Success(new NotificationConfig()
					{
						Message = "Device status update",
						Description = $"Device status change {deviceId} ONLINE.",
						Duration = 0.750
					});
				}
				else
				{
					await this.notificationService.Warning(new NotificationConfig()
					{
						Message = "Device status update",
						Description = $"Device status change {deviceId} OFFLINE.",
						Duration = 0.750
					});
				}

				dispatcher.Dispatch(new PatchDeviceState(deviceId, state));
			});

			hubConnection.On<string, string>("UpdateDevicePicture", (deviceId, picture) =>
			{
				dispatcher.Dispatch(new PatchDevicePicture(deviceId, picture));
			});

			hubConnection.On("FirmwareUpdate", () =>
			{
				dispatcher.Dispatch(new LoadFirmwareAction());
			});

			hubConnection.On<string, string[]>("UpdateDeviceSubscriptionState", (deviceId, topics) =>
			{
				dispatcher.Dispatch(new PatchDeviceSubscriptionState(deviceId, topics));
			});

			hubConnection.On<string, DeviceItemResponse>("UpdateDeviceListState", (deviceId, device) =>
			{
				dispatcher.Dispatch(new PatchDeviceListState(device));
			});

			hubConnection.On<string, GpsDataResponse>("GpsUpdate", (deviceId, gpsData) =>
			{
				dispatcher.Dispatch(new SetDevicePositionAction(gpsData));
				Console.WriteLine($"Latitude: {gpsData.Latitude} - Longitude:{gpsData.Longitude}");
			});

			this.dispatcher = dispatcher;
		}

		public async Task StartListen()
		{
			await hubConnection.StartAsync();
		}
	}
}
