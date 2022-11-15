using Android.App;
using Android.Content.PM;
using Android.Net.Wifi;
using Android.OS;
using Android.Content;
using Android.Runtime;
using Android.Media.Midi;

namespace DeviceConfigurator
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public void Test()
        {
            WifiNetworkSpecifier.Builder builder = new WifiNetworkSpecifier.Builder();
            var wifiManager = GetSystemService(WifiService).JavaCast<WifiManager>();
            var wifiMgr = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
        }
    }
}