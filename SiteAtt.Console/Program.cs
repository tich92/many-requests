﻿using Core;
using Core.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = CreateHostBuilder().Build();

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
            await service.ExecuteAsync(logger);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        await Task.Delay(TimeSpan.FromMinutes(2));
    }
}

static IHostBuilder CreateHostBuilder(params string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddServices();
        })
        .ConfigureLogging((_, logging) =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });