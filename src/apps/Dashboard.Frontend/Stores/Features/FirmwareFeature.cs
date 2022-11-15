using Dashboard.Frontend.Stores.States;
using Fluxor;

namespace Dashboard.Frontend.Stores.Features
{
    public class FirmwareFeature : Feature<FirmwareState>
    {
        public override string GetName() => "FirmwareState";
        protected override FirmwareState GetInitialState() =>
          new FirmwareState(null);
    }
}
