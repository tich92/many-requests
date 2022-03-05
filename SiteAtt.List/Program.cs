using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SiteAtt.List.Abstractions;
using SiteAtt.List.Services;

namespace SiteAtt.List ;

    public static class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults(builder =>
                {
                    builder.UseNewtonsoftJson();
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<ISitesService, SitesService>();
                    services.AddSingleton<ISitesStore, SitesStore>();
                    services.AddSingleton<MongoDbProvider>();
                })
                .ConfigureAppConfiguration(options =>
                {
                    options.SetBasePath(Directory.GetCurrentDirectory());
                    options.AddEnvironmentVariables();
                    options.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureOpenApi()
                .Build();

            host.Run();
        }
    }