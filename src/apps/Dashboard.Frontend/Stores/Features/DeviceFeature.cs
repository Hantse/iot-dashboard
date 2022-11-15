using Dashboard.Frontend.Stores.States;
using Fluxor;

namespace Dashboard.Frontend.Stores.Features
{
    public class DeviceFeature : Feature<DeviceState>
    {
        public override string GetName() => "DeviceState";
        protected override DeviceState GetInitialState() =>
          new DeviceState(null);
    }
}
