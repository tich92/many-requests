using System;
using System.Net;
using System.Threading.Tasks;
using SiteAtt.List.Abstractions;
using SiteAtt.List.Models;

namespace SiteAtt.List.Services ;

    public class SitesService : ISitesService
    {
        private readonly ISitesStore _sitesStore;
        
        public SitesService(ISitesStore sitesStore)
        {
            _sitesStore = sitesStore;
        }

        public async Task CreateOrUpdateSitesAsync(CreateSitesRequest request)
        {
            var sitesModel = await _sitesStore.GetListAsync();

            foreach (var site in request.Sites)
            {
                var isUrl = Uri.TryCreate(site, UriKind.Absolute, out _);
                var isIpAddress = IPAddress.TryParse(site, out _);

                if (isUrl || isIpAddress)
                {
                    sitesModel.Sites.Add(site);
                }
            }

            await _sitesStore.StoreAsync(sitesModel);
        }
        
        public Task<SitesModel> GetSitesAsync()
        {
            return _sitesStore.GetListAsync();
        }
    }