using System.Linq;
using System.Threading.Tasks;
using Core.Abstractions;
using Microsoft.Extensions.Logging;

namespace Core;

public class BombardingExecutionService : IBombardingExecutionService
{
    private readonly ISiteBombardService _siteBombardService;
    private readonly ISiteRetriever _siteRetriever;

    public BombardingExecutionService(ISiteBombardService siteBombardService, ISiteRetriever siteRetriever)
    {
        _siteBombardService = siteBombardService;
        _siteRetriever = siteRetriever;
    }

    public async Task ExecuteAsync(ILogger logger)
    {
        logger.LogInformation("Executing process ...");
        var sites = _siteRetriever.GetSites();

        var tasks = sites.Select(x => _siteBombardService.BombardAsync(x, logger));

        await Task.WhenAll(tasks);

        logger.LogInformation("Process executed");
    }
}