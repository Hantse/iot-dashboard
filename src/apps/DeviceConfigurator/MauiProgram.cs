using DeviceConfigurator.Interfaces;
using DeviceConfigurator.Services;
using Microsoft.Extensions.Logging;

namespace DeviceConfigurator
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<IWifiManagerService, WifiManagerService>();
            builder.Services.AddSingleton<ILocalDeviceService, LocalDeviceService>();
            builder.Services.AddAntDesign();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}