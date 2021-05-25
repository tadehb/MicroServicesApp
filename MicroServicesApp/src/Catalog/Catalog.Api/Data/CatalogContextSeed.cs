using Catalog.Api.Entities;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Catalog.Api.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> products)
        {
            bool exists = products.Find(x => true).Any();
            if (!exists)
            {
                products.InsertManyAsync(GetPreConfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreConfiguredProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Name = "IPhone X",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years.",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years.",
                    ImageFile = "product-1.png",
                    Price = 950.00M,
                    Category = "SmartPhone"
                },
                new Product
                {
                    Name = "Samsung Galaxy",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years.",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years.",
                    ImageFile = "product-1.png",
                    Price = 850.00M,
                    Category = "SmartPhone"
                },
                new Product
                {
                    Name = "Xiaomi Mi 9",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years.",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years.",
                    ImageFile = "product-1.png",
                    Price = 470.00M,
                    Category = "SmartPhone"
                },
                new Product
                {
                    Name = "HTC U11+ Plus",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years.",
                    Description = "This phone is the company's biggest change to its flagship smartphone in years.",
                    ImageFile = "product-1.png",
                    Price = 380.00M,
                    Category = "SmartPhone"
                }
            };
        }
    }
}