using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Abstractions;

public interface ISiteBombardService
{
    Task<bool> MakeRequestAsync(string siteUrl, HttpMessageInvoker client);
}