using System.Threading.Tasks;
using SiteAtt.List.Models;

namespace SiteAtt.List.Abstractions ;

    public interface ISitesStore
    {
        Task StoreAsync(SitesModel model);
        Task<SitesModel> GetListAsync();
    }