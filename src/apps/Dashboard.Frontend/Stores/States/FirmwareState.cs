using Dashboard.Frontend.Core;
using Service.Shared.Contracts.Response;
using System.Collections.Generic;

namespace Dashboard.Frontend.Stores.States
{
    public class FirmwareState : CoreState
    {
        public FirmwareState(IEnumerable<FirmwareItemResponse> firmwares, bool loadInProgress = false)
            : base(loadInProgress)
        {
            Firmwares = firmwares;
        }

        public FirmwareState(bool loadInProgress)
           : base(loadInProgress)
        {
        }

        public FirmwareState(bool loadInProgress, string error, bool isError)
            : base(loadInProgress, error, isError)
        {
        }

        public FirmwareState(IEnumerable<FirmwareItemResponse> firmwares, bool loadInProgress, string error, bool isError, FirmwareItemResponse firmware)
           : this(loadInProgress, error, isError)
        {
            Firmwares = firmwares;
            Firmware = firmware;
        }

        public IEnumerable<FirmwareItemResponse> Firmwares { get; }
        public FirmwareItemResponse Firmware { get; }
    }
}
