using Infrastructure.Core.Extensions;
using Infrastructure.Core.Interfaces;
using Infrastructure.Core.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MQTTnet.AspNetCore;
using MQTTnet.AspNetCore.AttributeRouting;
using MQTTnet.AspNetCore.Extensions;
using MQTTnet.Client.Receiving;
using MQTTnet.Diagnostics;
using MQTTnet.Server;
using Server.Broker.Business;
using Server.Broker.Hubs;
using Server.Broker.Infrastructure.Handlers;
using Server.Broker.Interfaces;
using Server.Broker.Repositories;
using Server.Broker.Security;
using Server.Broker.Services;
using System;
using System.IO;
using System.Linq;

namespace Server.Broker
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMqttControllers();
            services.ApplyDatabaseMigration(Configuration)
                    .ConfigureBaseService("IOT.Server.Broker")
                    .AddOptions();

            services
                .AddHostedMqttServer(mqttServer => mqttServer.WithoutDefaultEndpoint())
                .AddHostedMqttServerWithServices(mqttServer =>
                {
                    mqttServer.WithStorage(new RetainedMessageHandler());
                    mqttServer.WithoutDefaultEndpoint();
                    mqttServer.WithAttributeRouting();
                    mqttServer.WithDefaultCommunicationTimeout(TimeSpan.FromSeconds(1));
                })
                .AddMqttConnectionHandler()
                .AddConnections();

            services.AddCors(cors =>
            {
                cors.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader();
                });
            });

            services.AddSignalR();

            AddRepositories(services);
            AddBusiness(services);
            AddHandlers(services);
            AddServices(services);
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IDatabaseConnectionFactory, SqlDbConnectionFactory>();
            services.AddScoped<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IDeviceLogRepository, DeviceLogRepository>();
            services.AddScoped<IDeviceTopicSubscriptionRepository, DeviceTopicSubscriptionRepository>();
            services.AddScoped<IDeviceFileRepository, DeviceFileRepository>();
            services.AddScoped<IWorkerRepository, WorkerRepository>();
            services.AddScoped<IFirmwareRepository, FirmwareRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IDeviceValueRepository, DeviceValueRepository>();
            services.AddScoped<IGpsDataRepository, GpsDataRepository>();
            services.AddScoped<IFirmwareVersionRepository, FirmwareVersionRepository>();
        }

        private static void AddBusiness(IServiceCollection services)
        {
            services.AddScoped<IDeviceBusiness, DeviceBusiness>();
            services.AddScoped<IFirmwareBusiness, FirmwareBusiness>();
            services.AddScoped<IWorkerBusiness, WorkerBusiness>();
            services.AddScoped<IMessageBusiness, MessageBusiness>();
            services.AddScoped<IDeviceValueBusiness, DeviceValueBusiness>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IDeviceLogService, DeviceLogService>();
        }

        private static void AddHandlers(IServiceCollection services)
        {
            services.AddScoped<IMqttServerClientConnectedHandler, OnClientConnectedHandler>();
            services.AddScoped<IMqttServerClientDisconnectedHandler, OnClientDisconnectedHandler>();
            services.AddScoped<IMqttServerClientSubscribedTopicHandler, OnClientSubscribeTopicHandler>();
            services.AddScoped<IMqttApplicationMessageReceivedHandler, OnMessageHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IMqttServerClientConnectedHandler onConnectedHandler,
            IMqttServerClientDisconnectedHandler onDisconnectedHandler,
            IMqttServerClientSubscribedTopicHandler onClientSubscribeTopicHandler,
            IMqttApplicationMessageReceivedHandler onMessageHandler)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IOT.Server.Broker v1"));

            app.UseCors();
            app.UseRouting();

            StartApplication(app, onConnectedHandler, onDisconnectedHandler, onClientSubscribeTopicHandler, onMessageHandler);
        }

        private static void StartApplication(IApplicationBuilder app, IMqttServerClientConnectedHandler onConnectedHandler, IMqttServerClientDisconnectedHandler onDisconnectedHandler, IMqttServerClientSubscribedTopicHandler onClientSubscribeTopicHandler, IMqttApplicationMessageReceivedHandler onMessageHandler)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHub<DeviceHub>("/device");
                endpoints.MapConnectionHandler<MqttConnectionHandler>("/mqtt",
                    httpConnectionDispatcherOptions => httpConnectionDispatcherOptions.WebSockets.SubProtocolSelector =
                                                           protocolList =>
                                                               protocolList.FirstOrDefault() ?? string.Empty);

            });

            app.UseMqttServer((server) =>
            {
                server.Options.TlsEndpointOptions.IsEnabled = true;
                server.Options.TlsEndpointOptions.CertificateProvider = new CertificateProvider();
                server.ClientConnectedHandler = onConnectedHandler;
                server.ClientDisconnectedHandler = onDisconnectedHandler;
                server.ClientSubscribedTopicHandler = onClientSubscribeTopicHandler;
                server.ApplicationMessageReceivedHandler = onMessageHandler;
            });
        }
    }
}