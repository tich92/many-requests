using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Abstractions;

public interface ISiteRetriever
{
    Task<IEnumerable<string>> GetSitesAsync();
}