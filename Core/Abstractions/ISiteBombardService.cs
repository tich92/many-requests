using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Core.Abstractions;

public interface ISiteBombardService
{
    Task BombardAsync(string siteUrl, ILogger logger);
}