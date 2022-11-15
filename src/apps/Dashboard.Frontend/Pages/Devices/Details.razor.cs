using AntDesign;
using Dashboard.Frontend.Interfaces;
using Dashboard.Frontend.Stores.Actions.Device;
using Dashboard.Frontend.Stores.States;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Service.Shared.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Pages.Devices
{
    public partial class Details : ComponentBase
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        private IState<DeviceState> DeviceState { get; set; }

        [Inject]
        private IDispatcher Dispatcher { get; set; }

        [Inject]
        private IDeviceService DeviceService { get; set; }


        private IEnumerable<DeviceLogItemResponse> logs;

        private bool lastState;
        private bool isIdentify;

        private string commandTester = "directionnalController";

        private Dictionary<string, string> mappedValues = null;

        protected override async Task OnInitializedAsync()
        {
            logs = await DeviceService.GetDeviceLogsAsync(Id);
            DeviceState.StateChanged += DeviceState_StateChanged;
            Dispatcher.Dispatch(new SetSelectedDeviceAction(Id));
        }

        private async void DeviceState_StateChanged(object sender, EventArgs e)
        {
            StateHasChanged();
        }

        void handleChange(string value)
        {
            commandTester = value;
        }

        private string GetIcone()
        {
            if (lastState)
                return IconType.Outline.Poweroff;

            return IconType.Outline.Pause;
        }

        private string GetType()
        {
            if (lastState)
                return ButtonType.Primary;

            return ButtonType.Dashed;
        }

        private async Task ShootDeviceAsync(string deviceId)
        {
            await DeviceService.SendDeviceMessageAsync("mqtt/device/" + deviceId + "/commands/*", JsonSerializer.Serialize(new
            {
                command = "CaptureCamera"
            }));
            lastState = false;
        }

        private async Task IdentifyDeviceAsync(string deviceId)
        {
            isIdentify = !isIdentify;
            await DeviceService.SendDeviceMessageAsync("mqtt/device/" + deviceId + "/identify", JsonSerializer.Serialize(new { value = isIdentify ? 0 : 1 }));
        }

        private async Task ToggleFlashlight(string deviceId)
        {
            lastState = !lastState;

            await DeviceService.SendDeviceMessageAsync("mqtt/device/" + deviceId + "/commands/*", JsonSerializer.Serialize(new
            {
                command = "flash",
                value = (lastState) ? 1 : 0
            }));
        }

        private string GetDataLabels(string key, string value)
        {
            if (key == "freeHeap" || key == "spi_flash_size")
                return $"{int.Parse(value) / 1000}kb";

            return value;
        }
    }
}
