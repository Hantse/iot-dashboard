using Dashboard.Frontend.Interfaces;
using Dashboard.Frontend.Stores.Actions.Device;
using Dashboard.Frontend.Stores.States;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Service.Shared.Contracts.Response;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Components.Device
{
    public partial class DeviceList : ComponentBase
    {
        [Inject]
        private IState<DeviceState> DeviceState { get; set; }

        [Inject]
        private IDispatcher Dispatcher { get; set; }

        [Inject]
        private IDeviceService DeviceService { get; set; }

        private bool visible = false;

        private DeviceItemResponse deviceModal;
        private string selectedTopic;
        private string message = "{}";

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Dispatcher.Dispatch(new LoadDeviceAction());
            DeviceState.StateChanged += DeviceState_StateChanged;
        }

        private void DeviceState_StateChanged(object sender, EventArgs e)
        {
            StateHasChanged();
        }

        private async Task SendMessageAsync(DeviceItemResponse device, string topic)
        {
            deviceModal = device;
            selectedTopic = topic;
            visible = true;
        }

        private async Task HandleOk(MouseEventArgs e)
        {
            await DeviceService.SendDeviceMessageAsync(selectedTopic, message);
        }

        private async Task ShootDeviceAsync(string deviceId)
        {
            await DeviceService.SendDeviceMessageAsync("mqtt/device/" + deviceId  + "/commands/*", JsonSerializer.Serialize(new
            {
                command = "CaptureCamera"
            }));
        }

        private void HandleCancel(MouseEventArgs e)
        {
            visible = false;
        }

        private void onChange(string value)
        {
            message = value;
        }
    }
}
