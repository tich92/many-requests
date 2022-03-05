using System;
using System.Security.Authentication;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace SiteAtt.List.Services ;

    public class MongoDbProvider
    {
        private readonly IConfiguration _configuration;
        
        public MongoDbProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (IMongoDatabase, IMongoCollection<T>) GetCollection<T>(string collectionName)
        {
            var connectionString = _configuration["MONGO_CONNECTION"];
            var database = GetDatabase(connectionString, "SiteAtt");

            var collection = database.GetCollection<T>(collectionName);
            return (database, collection);
        }

        private static IMongoDatabase GetDatabase(string connectionString, string databaseName)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
            settings.MaxConnectionPoolSize = 500;
            settings.RetryWrites = false;

            var client = new MongoClient(settings);

            var database = client.GetDatabase(databaseName);

            if (database == null)
            {
                throw new ArgumentNullException(nameof(database));
            }

            return database;
        }
    }