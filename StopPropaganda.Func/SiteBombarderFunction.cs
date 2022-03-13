using Core.Abstractions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace StopPropaganda.Func
{
    public class SiteBombarderFunction
    {
        private readonly ILogger _logger;
        private readonly IBombardingExecutionService _bombardingExecutionService;

        public SiteBombarderFunction(ILoggerFactory loggerFactory, IBombardingExecutionService bombardingExecutionService)
        {
            _bombardingExecutionService = bombardingExecutionService;
            _logger = loggerFactory.CreateLogger<SiteBombarderFunction>();
        }

        [Function("SiteBombarderFunction0")]
        public async Task Run([TimerTrigger("0 */1 * * * *", RunOnStartup = true)] MyInfo myTimer)
        {
            _logger.LogInformation($"Execution started: {DateTime.Now}");

            await _bombardingExecutionService.ExecuteAsync();

            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
    }
}