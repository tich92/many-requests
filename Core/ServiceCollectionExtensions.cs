using System.Net.Http;
using Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpClient("sites")
            .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = System.Net.DecompressionMethods.GZip
            });

        services.AddSingleton<ISiteBombardService, SiteBombardService>();
        services.AddSingleton<IBombardingExecutionService, BombardingExecutionService>();
        services.AddSingleton<ISiteRetriever, SiteRetriever>();

        return services;
    }
}