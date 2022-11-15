using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MQTTnet.AspNetCore.Extensions;

namespace Server.Broker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(
                        o =>
                        {
                            o.ListenAnyIP(1883, l => l.UseMqtt());
                            o.ListenAnyIP(5001);
                            //o.ListenAnyIP(6003, o => o.UseHttps());
                        });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
