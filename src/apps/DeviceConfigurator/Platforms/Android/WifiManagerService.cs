using Android.Content;
using Android.Net.Wifi;
using Android.Provider;
using DeviceConfigurator.Platforms.Android;

namespace DeviceConfigurator.Services
{
    public partial class WifiManagerService
    {
        private Context context = null;

        public WifiManagerService()
        {
            context = Android.App.Application.Context;
        }

        public partial async Task<IEnumerable<string>> GetAvailableNetworksAsync()
        {
            IEnumerable<string> availableNetworks = null;
            var wifiMgr = (WifiManager)context.GetSystemService(Context.WifiService);
            var wifiReceiver = new WifiReceiver(wifiMgr);

            if (!wifiMgr.IsWifiEnabled)
            {
                Intent wifiSettingsIntent = new Intent(Settings.Panel.ActionWifi);
                wifiSettingsIntent.SetFlags(ActivityFlags.NewTask);
                context.StartActivity(wifiSettingsIntent);
            }

            await Task.Run(() =>
            {
                // Start a scan and register the Broadcast receiver to get the list of Wifi Networks
                context.RegisterReceiver(wifiReceiver, new IntentFilter(WifiManager.ScanResultsAvailableAction));
                availableNetworks = wifiReceiver.Scan();
            });

            return availableNetworks;
        }
    }
}
