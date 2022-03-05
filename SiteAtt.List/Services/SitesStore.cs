using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using SiteAtt.List.Abstractions;
using SiteAtt.List.Models;

namespace SiteAtt.List.Services ;

    public class SitesStore : ISitesStore
    {
        private readonly IMongoCollection<StoredSite> _sitesCollection;
        private readonly IMongoDatabase _mongoDatabase;

        public SitesStore(MongoDbProvider mongoDbProvider)
        {
            var (database, collection) = mongoDbProvider.GetCollection<StoredSite>("Sites");

            _mongoDatabase = database;
            _sitesCollection = collection;
        }

        public async Task StoreAsync(SitesModel model)
        {
            var storedSites = model.Sites.Select(site => new StoredSite
            {
                Id = Guid.NewGuid(),
                Url = site
            }).ToList();
            
            using var session = await _mongoDatabase.Client.StartSessionAsync();
            session.StartTransaction();

            try
            {
                await _sitesCollection.DeleteManyAsync(session, FilterDefinition<StoredSite>.Empty);

                await _sitesCollection.InsertManyAsync(session, storedSites);

                await session.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await session.AbortTransactionAsync();
            }
        }
        
        public async Task<SitesModel> GetListAsync()
        {
            var storedSites = await _sitesCollection.Find(FilterDefinition<StoredSite>.Empty).ToListAsync();

            var model = new SitesModel();

            foreach (var site in storedSites)
            {
                model.Sites.Add(site.Url);
            }

            return model;
        }
    }