using Dashboard.Frontend.Core;
using Service.Shared.Contracts.Response;
using System;
using System.Collections.Generic;

namespace Dashboard.Frontend.Stores.Actions.Device
{
    public class LoadDeviceAction
    {

    }

    public class LoadDeviceFailureAction : CoreFailureAction
    {
        public LoadDeviceFailureAction(string error, bool isError)
            : base(error, isError)
        {
        }
    }

    public class LoadDeviceSuccessAction
    {
        public IEnumerable<DeviceItemResponse> Devices { get; }

        public LoadDeviceSuccessAction(IEnumerable<DeviceItemResponse> devices)
        {
            Devices = devices;
        }
    }

    public class PatchDeviceState
    {
        public PatchDeviceState(string deviceId, bool state)
        {
            DeviceId = deviceId;
            State = state;
        }

        public string DeviceId { get; }
        public bool State { get; }
    }

    public class PatchDeviceSuccessState
    {
        public IEnumerable<DeviceItemResponse> Devices { get; }

        public PatchDeviceSuccessState(IEnumerable<DeviceItemResponse> devices)
        {
            Devices = devices;
        }
    }

    public class PatchDeviceSubscriptionState
    {
        public PatchDeviceSubscriptionState(string deviceId, string[] topics)
        {
            DeviceId = deviceId;
            Topics = topics;
        }

        public string DeviceId { get; }
        public string[] Topics { get; }
    }

    public class PatchDeviceSubscriptionSuccessState
    {
        public IEnumerable<DeviceItemResponse> Devices { get; }

        public PatchDeviceSubscriptionSuccessState(IEnumerable<DeviceItemResponse> devices)
        {
            Devices = devices;
        }
    }

    public class PatchDeviceListState
    {
        public DeviceItemResponse Device { get; }

        public PatchDeviceListState(DeviceItemResponse device)
        {
            Device = device;
        }
    }

    public class PatchDeviceListSuccessState
    {
        public IEnumerable<DeviceItemResponse> Devices { get; }

        public PatchDeviceListSuccessState(IEnumerable<DeviceItemResponse> devices)
        {
            Devices = devices;
        }
    }

    public class PatchDevicePicture
    {
        public PatchDevicePicture(string deviceId, string imgSource)
        {
            DeviceId = deviceId;
            ImgSource= imgSource;
        }

        public string DeviceId { get; }
        public string ImgSource { get; }
    }

    public class PatchDeviceSuccessPicture
    {
        public IEnumerable<DeviceItemResponse> Devices { get; }

        public PatchDeviceSuccessPicture(IEnumerable<DeviceItemResponse> devices)
        {
            Devices = devices;
        }
    }

    public class SetSelectedDeviceAction
    {
        public SetSelectedDeviceAction(Guid deviceId)
        {
            DeviceId = deviceId;
        }

        public Guid DeviceId { get; set; }
    }

    public class SetSelectedDeviceSuccessAction
    {
        public SetSelectedDeviceSuccessAction(DeviceItemResponse device)
        {
            Device = device;
        }

        public DeviceItemResponse Device { get; set; }
    }
}
