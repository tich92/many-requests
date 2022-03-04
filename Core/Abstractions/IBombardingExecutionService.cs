using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Core.Abstractions;

public interface IBombardingExecutionService
{
    Task ExecuteAsync(ILogger logger);
}