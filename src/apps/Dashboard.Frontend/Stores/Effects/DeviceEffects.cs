using Dashboard.Frontend.Interfaces;
using Dashboard.Frontend.Stores.Actions.Device;
using Dashboard.Frontend.Stores.States;
using Fluxor;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard.Frontend.Stores.Effects
{
    public class DeviceEffects
    {
        private readonly IDeviceService deviceService;

        private readonly IState<DeviceState> deviceState;

        public DeviceEffects(IDeviceService deviceService, IState<DeviceState> deviceState)
        {
            this.deviceService = deviceService;
            this.deviceState = deviceState;
        }

        [EffectMethod]
        public async Task HandleLoadDeviceActionAction(LoadDeviceAction action, IDispatcher dispatcher)
        {
            var devices = await deviceService.GetDevicesAsync();
            dispatcher.Dispatch(new LoadDeviceSuccessAction(devices));
        }

        [EffectMethod]
        public async Task HandlePatchDeviceAction(PatchDeviceState action, IDispatcher dispatcher)
        {
            if (deviceState.Value.Devices != null)
            {
                var devices = deviceState.Value.Devices;
                var device = devices.FirstOrDefault(f => f.Name == action.DeviceId);

                if (deviceState.Value.Device != null && deviceState.Value.Device.Name == action.DeviceId)
                {
                    deviceState.Value.Device.Online = action.State;
                }

                if (device == null)
                {
                    dispatcher.Dispatch(new LoadDeviceAction());
                }
                else
                {
                    device.Online = action.State;
                    dispatcher.Dispatch(new PatchDeviceSuccessState(devices));
                }
            }
        }

        [EffectMethod]
        public async Task HandlePatchDevicePictureAction(PatchDevicePicture action, IDispatcher dispatcher)
        {
            if (deviceState.Value.Devices != null)
            {
                var devices = deviceState.Value.Devices;
                var device = devices.FirstOrDefault(f => f.Name == action.DeviceId);
                if (device == null)
                {
                    dispatcher.Dispatch(new LoadDeviceAction());
                }
                else
                {
                    if (deviceState.Value?.Device?.Name == action.DeviceId)
                    {
                        deviceState.Value.Device.LastPicture = action.ImgSource;
                    }

                    device.LastPicture = action.ImgSource;
                    dispatcher.Dispatch(new PatchDeviceSuccessPicture(devices));
                }
            }
        }

        [EffectMethod]
        public async Task HandlePatchDevicePictureAction(PatchDeviceSubscriptionState action, IDispatcher dispatcher)
        {
            if (deviceState.Value.Devices != null)
            {
                var devices = deviceState.Value.Devices;
                var device = devices.FirstOrDefault(f => f.Name == action.DeviceId);
                if (device == null)
                {
                    dispatcher.Dispatch(new LoadDeviceAction());
                }
                else
                {
                    device.Topics = action.Topics;
                    dispatcher.Dispatch(new PatchDeviceSubscriptionSuccessState(devices));
                }
            }
        }

        [EffectMethod]
        public async Task HandlePatchDeviceListAction(PatchDeviceListState action, IDispatcher dispatcher)
        {
            if (deviceState.Value.Devices != null)
            {
                if (action.Device != null)
                {
                    var devices = deviceState.Value.Devices.Prepend(action.Device);
                    dispatcher.Dispatch(new PatchDeviceListSuccessState(devices));
                }
            }
        }

        [EffectMethod]
        public async Task HandleSetDeviceAction(SetSelectedDeviceAction action, IDispatcher dispatcher)
        {
            if (deviceState.Value.Devices == null || !deviceState.Value.Devices.Any())
                dispatcher.Dispatch(new LoadDeviceAction());

            var device = await deviceService.GetDeviceAsync(action.DeviceId);
            dispatcher.Dispatch(new SetSelectedDeviceSuccessAction(device));
        }
    }
}
