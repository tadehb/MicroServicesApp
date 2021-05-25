﻿using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;
using Catalog.Api.Settings;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }

        public CatalogContext(ICatalogDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            Products = database.GetCollection<Product>(settings.CollectionName);

            CatalogContextSeed.SeedData(Products);
        }
    }
}
