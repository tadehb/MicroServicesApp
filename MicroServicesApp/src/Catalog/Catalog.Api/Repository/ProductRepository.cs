using Catalog.Api.Data.Interfaces;
using Catalog.Api.Entities;
using Catalog.Api.Repository.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Repository
{
    public class ProductRepository : IProductRepository
    {
        public readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            this._context = context;
        }

        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }


        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context
                .Products
                .Find(x => true).ToListAsync();

        }

        public async Task<Product> GetProductById(string id)
        {
            return await _context
                .Products
                .Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string category)
        {
            //FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(x => x.Category, category);
            return await _context
                .Products
                .Find(x=>x.Category == category).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.ElemMatch(x => x.Name, name);
            return await _context
                .Products
                .Find(filter).ToListAsync();
        }


        public async Task<bool> Update(Product product)
        {
            var updateResult = await _context.Products.ReplaceOneAsync(filter: x => x.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            DeleteResult deleteResult = await _context.Products.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }
    }
}
