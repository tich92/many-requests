using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Abstractions;
using Microsoft.Extensions.Logging;

namespace Core;

public class SiteBombardService : ISiteBombardService
{
    private readonly ILogger<SiteBombardService> _logger;

    public SiteBombardService(ILogger<SiteBombardService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> MakeRequestAsync(string siteUrl, HttpMessageInvoker client, CancellationToken cancellationToken)
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, new Uri(siteUrl));
            
            request.Headers.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.51 Safari/537.36");
            request.Headers.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

            request.Headers.TryAddWithoutValidation("sec-ch-ua", " Not A;Brand\";v=\"99\", \"Chromium\";v=\"99\", \"Google Chrome\";v=\"99\"");
            request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
            request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "Windows");
            request.Headers.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");

            var result = await client.SendAsync(request, cancellationToken);

            return result.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $@"exception on: {siteUrl}. message: {e.Message}");
            return false;
        }
    }
}