using AntDesign;
using Dashboard.Frontend.Interfaces;
using Dashboard.Frontend.Stores.States;
using Fluxor;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using static Dashboard.Frontend.Stores.Actions.Firmware.FirmwareAction;

namespace Dashboard.Frontend.Components.Firmware
{
    public partial class FirmwareList : ComponentBase
    {
        [Inject]
        private IState<FirmwareState> FirmwareState { get; set; }

        [Inject]
        private IDispatcher Dispatcher { get; set; }

        [Inject]
        private IFirmwareService FirmwareService { get; set; }

        [Inject]
        private ILocalModalService LocalModalService { get; set; }

        private ModalRef deployModalRef;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Dispatcher.Dispatch(new LoadFirmwareAction());
            FirmwareState.StateChanged += FirmwareState_StateChanged;
        }

        private async Task OpenDeployDialog(Guid selectedFirmware)
        {
            deployModalRef = await LocalModalService.ShowDeployModalAsync(selectedFirmware);
        }

        private void FirmwareState_StateChanged(object sender, EventArgs e)
        {
            StateHasChanged();
        }
    }
}
