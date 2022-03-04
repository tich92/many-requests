using Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddSingleton<ISiteBombardService, SiteBombardService>();
        services.AddSingleton<IBombardingExecutionService, BombardingExecutionService>();
        services.AddSingleton<ISiteRetriever, SiteRetriever>();

        return services;
    }
}