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
        "https://iecp.ru/ep/ep-verification",
        "https://iecp.ru/ep/uc-list",
        "https://uc-osnovanie.ru/",
        "http://www.nucrf.ru",
        "http://www.belinfonalog.ru",
        "http://www.roseltorg.ru",
        "http://www.astralnalog.ru",
        "http://www.nwudc.ru",
        "http://www.center-inform.ru",
        "https://kk.bank/UdTs",
        "http://structure.mil.ru/structure/uc/info.htm",
        "http://www.ucpir.ru",
        "http://dreamkas.ru",
        "http://www.e-portal.ru",
        "http://izhtender.ru",
        "http://imctax.parus-s.ru",
        "http://www.icentr.ru",
        "http://www.kartoteka.ru",
        "http://rsbis.ru/elektronnaya-podpis",
        "http://www.stv-it.ru",
        "http://www.crypset.ru",
        "http://www.kt-69.ru",
        "http://www.24ecp.ru",
        "http://kraskript.com",
        "http://ca.ntssoft.ru",
        "http://www.y-center.ru",
        "http://www.rcarus.ru",
        "http://rk72.ru",
        "http://squaretrade.ru",
        "http://ca.gisca.ru",
        "http://www.otchet-online.ru",
        "http://udcs.ru",
        "http://www.cit-ufa.ru",
        "http://elkursk.ru",
        "http://www.icvibor.ru",
        "http://ucestp.ru",
        "http://mcspro.ru",
        "http://www.infotrust.ru",
        "http://epnow.ru",
        "http://ca.kamgov.ru",
        "http://mascom-it.ru",
        "http://cfmc.ru",
        "https://gosuslugi41.ru", 
        "https://uslugi27.ru", 
        "https://gosuslugi29.ru", 
        "https://gosuslugi.astrobl.ru",
        "https://xn--90aivcdt6dxbc.xn--p1ai/",
        "https://savelife.pw/"
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