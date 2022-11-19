using Dashboard.Frontend.Stores.Actions.Device;
using Dashboard.Frontend.Stores.States;
using Fluxor;

namespace Dashboard.Frontend.Stores.Reducers.Device
{
	public static class DeviceReducers
	{
		[ReducerMethod]
		public static DeviceState LoadDeviceAction(DeviceState state, LoadDeviceAction action) =>
						new DeviceState(true);

		[ReducerMethod]
		public static DeviceState LoadDeviceSuccessAction(DeviceState state, LoadDeviceSuccessAction action) =>
				new DeviceState(action.Devices, false, null, false, state.Device, state.DevicePosition);

		[ReducerMethod]
		public static DeviceState LoadDeviceFailureAction(DeviceState state, LoadDeviceFailureAction action) =>
				new DeviceState(null, false, action.Error, action.IsError, null, state.DevicePosition);

		[ReducerMethod]
		public static DeviceState PatchDeviceStateAction(DeviceState state, PatchDeviceState action) =>
			new DeviceState(state.Devices, false, null, false, state.Device, state.DevicePosition);

		[ReducerMethod]
		public static DeviceState PatchDeviceStateSuccessAction(DeviceState state, PatchDeviceSuccessState action) =>
			new DeviceState(action.Devices, false, null, false, state.Device, state.DevicePosition);

		[ReducerMethod]
		public static DeviceState PatchDeviceSubscriptionStateAction(DeviceState state, PatchDeviceSubscriptionState action) =>
			new DeviceState(state.Devices, false, null, false, state.Device, state.DevicePosition);

		[ReducerMethod]
		public static DeviceState PatchDeviceSubscriptionStateSuccessAction(DeviceState state, PatchDeviceSubscriptionSuccessState action) =>
			new DeviceState(action.Devices, false, null, false, state.Device, state.DevicePosition);

		[ReducerMethod]
		public static DeviceState PatchDevicePictureAction(DeviceState state, PatchDevicePicture action) =>
			new DeviceState(state.Devices, false, null, false, state.Device, state.DevicePosition);

		[ReducerMethod]
		public static DeviceState PatchDevicePictureSuccessAction(DeviceState state, PatchDeviceSuccessPicture action) =>
			new DeviceState(action.Devices, false, null, false, state.Device, state.DevicePosition);

		[ReducerMethod]
		public static DeviceState PatchDeviceListAction(DeviceState state, PatchDevicePicture action) =>
			new DeviceState(state.Devices, false, null, false, state.Device, state.DevicePosition);

		[ReducerMethod]
		public static DeviceState PatchDeviceListSuccessAction(DeviceState state, PatchDeviceSuccessPicture action) =>
			new DeviceState(action.Devices, false, null, false, state.Device, state.DevicePosition);

		[ReducerMethod]
		public static DeviceState SetDeviceAction(DeviceState state, SetSelectedDeviceAction action) =>
			new DeviceState(state.Devices, false);

		[ReducerMethod]
		public static DeviceState SetDeviceSuccessAction(DeviceState state, SetSelectedDeviceSuccessAction action) =>
			new DeviceState(state.Devices, false, null, false, action.Device, state.DevicePosition);

		[ReducerMethod]
		public static DeviceState SetDevicePositionAction(DeviceState state, SetDevicePositionAction action) =>
			new DeviceState(state.Devices, state.LoadInProgress, state.Error, state.IsError, state.Device, action.GpsData);
	}
}
