using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.Abstractions;
using SiteAtt.List.Contracts;

namespace Core;

public class SiteRetriever : ISiteRetriever
{
    private const string SitesListApi = "https://sites-func.azurewebsites.net/api/sites";
    private readonly IHttpClientFactory _httpClientFactory;
    
    private readonly List<string> _sites = new()
    {
        "https://topwar.ru/",
        "https://president.gov.by",
        "https://my.bank-hlynov.ru",
        "https://link.centrinvest.ru",
        "https://chbrr.crimea.com",
        "https://enter.unicredit.ru"
    };

    public SiteRetriever(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<string>> GetSitesAsync()
    {
        var client = _httpClientFactory.CreateClient("sites-api");

        try
        {
            var response = await client.GetFromJsonAsync<SiteListDto>(SitesListApi);
            var sites = response?.Sites ?? _sites;

            return sites;
        }
        catch (Exception)
        {
            return _sites;
        }
    }
}