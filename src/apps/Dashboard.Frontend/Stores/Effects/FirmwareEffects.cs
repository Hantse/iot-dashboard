using Dashboard.Frontend.Interfaces;
using Dashboard.Frontend.Stores.States;
using Fluxor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Dashboard.Frontend.Stores.Actions.Firmware.FirmwareAction;

namespace Dashboard.Frontend.Stores.Effects
{
    public class FirmwareEffects
    {
        private readonly IFirmwareService firmwareService;

        private readonly IState<FirmwareState> firmwareState;

        public FirmwareEffects(IFirmwareService firmwareService, IState<FirmwareState> firmwareState)
        {
            this.firmwareService = firmwareService;
            this.firmwareState = firmwareState;
        }

        [EffectMethod]
        public async Task HandleLoadFirmwareActionAction(LoadFirmwareAction action, IDispatcher dispatcher)
        {
            var firmwares = await firmwareService.GetFirmwaresAsync();
            dispatcher.Dispatch(new LoadFirmwareSuccessAction(firmwares));
        }
    }
}
