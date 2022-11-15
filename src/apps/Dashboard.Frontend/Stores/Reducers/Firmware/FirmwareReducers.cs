using Dashboard.Frontend.Stores.States;
using Fluxor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Dashboard.Frontend.Stores.Actions.Firmware.FirmwareAction;

namespace Dashboard.Frontend.Stores.Reducers.Firmware
{
    public static class FirmwareReducers
    {
        [ReducerMethod]
        public static FirmwareState LoadFirmwareAction(FirmwareState state, LoadFirmwareAction action) =>
                new FirmwareState(true);

        [ReducerMethod]
        public static FirmwareState LoadFirmwareSuccessAction(FirmwareState state, LoadFirmwareSuccessAction action) =>
                new FirmwareState(action.Firmwares, false, null, false, state.Firmware);

        [ReducerMethod]
        public static FirmwareState LoadFirmwareFailureAction(FirmwareState state, LoadFirmwareFailureAction action) =>
                new FirmwareState(null, false, action.Error, action.IsError, null);
    }
}
