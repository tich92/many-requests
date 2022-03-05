using System.Threading.Tasks;
using SiteAtt.List.Models;

namespace SiteAtt.List.Abstractions ;

    public interface ISitesService
    {
        Task CreateOrUpdateSitesAsync(CreateSitesRequest request);
        Task<SitesModel> GetSitesAsync();
    }