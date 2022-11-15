using AntDesign;
using DeviceConfigurator.Contracts.Response;
using DeviceConfigurator.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DeviceConfigurator.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public ILocalDeviceService LocalDeviceService { get; set; }

        [Inject]
        public NotificationService NotificationService { get; set; }

        private bool deviceFound;
        private string deviceName;

        private string ssid;
        private string password = "PCSNLVOI";

        private List<AvailableNetworkResponse> availableNetworks;

        private System.Timers.Timer scanTimer;

        private async Task Configure()
        {
            var configureResult = await LocalDeviceService.ConfigureDevice(ssid, password);
            if (configureResult)
            {
                await NotificationService.Success(new NotificationConfig()
                {
                    Message = "Configuration",
                    Description = "Device configure."
                });
            }
        }

        void handleChange(string value)
        {
            ssid = value;
            StateHasChanged();
        }

        protected override void OnInitialized()
        {

            scanTimer = new System.Timers.Timer(2500);
            scanTimer.Elapsed += async (sender, args) =>
            {
                var scanResult = await LocalDeviceService.ScanDeviceAsync();
                deviceFound = scanResult.find;
                deviceName = scanResult.name;

                if (scanResult.find)
                {
                    availableNetworks = (await LocalDeviceService.ScanNetworks())?.AvailableNetworks;
                }

                await InvokeAsync(StateHasChanged);
            };

            scanTimer.AutoReset = true;
            scanTimer.Enabled = true;

            base.OnInitialized();
        }
    }
}
