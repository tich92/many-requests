using System.Collections.Generic;

namespace Core.Abstractions;

public interface ISiteRetriever
{
    IEnumerable<string> GetSites();
}