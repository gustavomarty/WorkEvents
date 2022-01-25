using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WorkEventsProcess;

IHostBuilder host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => {
        services.AddSingleton<ParametrosExecucaoServiceBus>(
            new ParametrosExecucaoServiceBus(){
                ConnectionString = "<<service-bus-connection>>",
                Topic = "team-topic",
                Subscription = "team-sub1"
            });
        services.AddHostedService<Worker>();
        
    });

host.Build().Run();