using Dashboard.Frontend.Core;
using Service.Shared.Contracts.Response;
using System.Collections.Generic;

namespace Dashboard.Frontend.Stores.States
{
    public class DeviceState : CoreState
    {
        public DeviceState(IEnumerable<DeviceItemResponse> devices, bool loadInProgress = false)
            : base(loadInProgress)
        {
            Devices = devices;
        }

        public DeviceState(bool loadInProgress)
            : base(loadInProgress)
        {
        }

        public DeviceState(bool loadInProgress, string error, bool isError)
            : base(loadInProgress, error, isError)
        {
        }

		public DeviceState(IEnumerable<DeviceItemResponse> devices, bool loadInProgress, string error, bool isError, DeviceItemResponse device, GpsDataResponse devicePosition)
		   : this(loadInProgress, error, isError)
		{
			Devices = devices;
			Device = device;
			DevicePosition = devicePosition;
		}


		public IEnumerable<DeviceItemResponse> Devices { get; }
        public DeviceItemResponse Device { get; }
        public GpsDataResponse DevicePosition { get; }

    }
}
