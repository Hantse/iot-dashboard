using AntDesign;
using Dashboard.Frontend.Interfaces;
using Dashboard.Frontend.Stores.Actions.Device;
using Dashboard.Frontend.Stores.States;
using Fluxor;
using Microsoft.AspNetCore.Components;
using Service.Shared.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Components.Firmware
{
    public partial class DeployModal : ComponentBase
    {
        [Parameter]
        public Guid SelectedFirmwareId { get; set; }

        [Inject]
        private IState<FirmwareState> FirmwareState { get; set; }

        [Inject]
        private IDispatcher Dispatcher { get; set; }

        [Inject]
        private IState<DeviceState> DeviceState { get; set; }

        [Inject]
        private ModalService ModalService { get; set; }

        [Inject]
        private IFirmwareService FirmwareService { get; set; }

        [Inject]
        public NotificationService Notice { get; set; }

        private IEnumerable<FirmwareVersionItemResponse> versions;
        private FirmwareItemResponse selectedFirmware;
        private List<string> selectedDevices = new List<string>();
        private bool loadInProgress;
        private Guid selectedVersionId;

        protected override async Task OnInitializedAsync()
        {
            versions = await FirmwareService.GetFirmwareVersionsAsync(SelectedFirmwareId);

            selectedFirmware = FirmwareState.Value.Firmwares.FirstOrDefault(f => f.Id == SelectedFirmwareId);
            if (DeviceState.Value.Devices == null)
            {
                Dispatcher.Dispatch(new LoadDeviceAction());
            }
            DeviceState.StateChanged += DeviceState_StateChanged;
        }

        public async Task Deploy()
        {
            loadInProgress = true;
            bool installResponse;
            if (selectedDevices.Count > 1)
            {
                installResponse = await FirmwareService.InstallOnDevicesAsync(selectedVersionId, new Service.Shared.Contracts.Request.InstallOnDeviceRequest()
                {
                    Devices = selectedDevices.ToArray()
                });
            }
            else
            {
                installResponse = await FirmwareService.InstallOnDeviceAsync(selectedVersionId, selectedDevices.First());
            }

            if (installResponse)
            {
                await ModalService.DestroyAllConfirmAsync();

                await Notice.Open(new NotificationConfig()
                {
                    Message = "Deploy in progress",
                    NotificationType = NotificationType.Success
                });
            }
            else
            {
                await Notice.Open(new NotificationConfig()
                {
                    Message = "Fail to deploy",
                    NotificationType = NotificationType.Error
                });
            }

            loadInProgress = false;
            StateHasChanged();
        }

        public void HandleValueChange(IEnumerable<string> values)
        {
            selectedDevices = values.ToList();
        }

        public void HandleVersionChange(string version)
        {
            selectedVersionId = Guid.Parse(version);
        }

        private string GetStatus(bool online)
        {
            return (online) ? "Online" : "Offline";
        }

        private void DeviceState_StateChanged(object sender, EventArgs e)
        {
            StateHasChanged();
        }
    }
}
