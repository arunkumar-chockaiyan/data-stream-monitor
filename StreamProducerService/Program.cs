using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using StreamProducerService;
using StreamProducerService.Configuration;
using StreamProducerService.Service;

internal class Program
{
    private static void Main(string[] args)
    {
        IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                AddApplicationServices(services);
                AddApplicationValidators(hostContext, services);
            })
            .Build();

        host.Run();
    }

    private static void AddApplicationServices(IServiceCollection services)
    {
        services.AddHostedService<Worker>();
        services.AddHttpClient<Worker>();
        services.AddSingleton<ITwitterStreamService, TwitterStreamService>();
    }

    private static void AddApplicationValidators(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.Configure<TwitterConfig>(hostContext.Configuration.GetSection("Twitter"));
        services.AddTransient(_ => _.GetRequiredService<IOptions<TwitterConfig>>().Value);
        services.AddSingleton<IValidateOptions<TwitterConfig>, TwitterConfigValidator>();

        services.Configure<MonitorServiceConfig>(hostContext.Configuration.GetSection("MonitorService"));
        services.AddTransient(_ => _.GetRequiredService<IOptions<MonitorServiceConfig>>().Value);
        services.AddSingleton<IValidateOptions<MonitorServiceConfig>, MonitorServiceConfigValidator>();
    }
}