using Dashboard.Frontend.Core;
using Service.Shared.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Stores.Actions.Firmware
{
    public class FirmwareAction
    {
        public class LoadFirmwareAction
        {

        }

        public class LoadFirmwareFailureAction : CoreFailureAction
        {
            public LoadFirmwareFailureAction(string error, bool isError)
                : base(error, isError)
            {
            }
        }

        public class LoadFirmwareSuccessAction
        {
            public IEnumerable<FirmwareItemResponse> Firmwares { get; }

            public LoadFirmwareSuccessAction(IEnumerable<FirmwareItemResponse> firmwares)
            {
                Firmwares = firmwares;
            }
        }
    }
}
