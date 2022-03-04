using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Abstractions;
using Microsoft.Extensions.Logging;

namespace Core;

public class SiteBombardService : ISiteBombardService
{
    private const int IterationsCount = 10000;
    private readonly IHttpClientFactory _httpClientFactory;

    public SiteBombardService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task BombardAsync(string siteUrl, ILogger logger)
    {
        var list = new List<Task>(IterationsCount);

        var client = _httpClientFactory.CreateClient($"Site - {siteUrl}");

        for (var i = 0; i < IterationsCount; i++)
        {
            var task = Task.Run(async () =>
            {
                try
                {
                    var result = await client.GetAsync(siteUrl);

                    logger.LogInformation($"Site: {siteUrl}. HTTP Result: {result.StatusCode}");
                }
                catch (Exception e)
                {
                    logger.LogError(e, $@"exception on: {siteUrl}. message: {e.Message}");
                }
            });

            list.Add(task);
        }

        await Task.WhenAll(list);
    }
}