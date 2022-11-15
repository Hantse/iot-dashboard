using Dashboard.Frontend.Interfaces;
using Dashboard.Frontend.Services;
using Fluxor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dashboard.Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5001/api/") });

            AddDependencies(builder);
            AddServices(builder);

            await builder.Build().RunAsync();
        }

        private static void AddDependencies(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddAntDesign();
            builder.Services.AddFluxor(o => o.ScanAssemblies(typeof(Program).Assembly).UseReduxDevTools());
        }

        private static void AddServices(WebAssemblyHostBuilder builder)
        {
            builder.Services.AddScoped<IDeviceService, DeviceService>();
            builder.Services.AddScoped<ILocalModalService, LocalModalService>();
            builder.Services.AddScoped<IFirmwareService, FirmwareService>();
            builder.Services.AddScoped<IRealtimeService, RealtimeService>();
        }
    }
}
