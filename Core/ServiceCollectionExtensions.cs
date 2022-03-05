using System;
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
            .ConfigureHttpClient(c => c.Timeout = TimeSpan.FromMinutes(2))
            .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = System.Net.DecompressionMethods.GZip,
            });

        services.AddScoped<ISiteBombardService, SiteBombardService>();
        services.AddScoped<IBombardingExecutionService, BombardingExecutionService>();
        services.AddSingleton<ISiteRetriever, SiteRetriever>();

        return services;
    }
}