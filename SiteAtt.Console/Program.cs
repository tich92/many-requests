using System.Net;
using Core;
using Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = CreateHostBuilder().Build();

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
ServicePointManager.Expect100Continue = false;

var serviceProvider = host.Services;

var service = serviceProvider.GetRequiredService<IBombardingExecutionService>();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Application started ...");

var counter = 0;

while (true)
{
    counter++;

    using (logger.BeginScope($"Started iteration: {counter}"))
    {
        try
        {
            await service.ExecuteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }   
    }
}

static IHostBuilder CreateHostBuilder(params string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddLogging(loggerBuilder =>
            {
                loggerBuilder.ClearProviders();
                loggerBuilder.AddConsole();
            });
            
            services.AddServices();
        })
        .ConfigureLogging((_, logging) =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });