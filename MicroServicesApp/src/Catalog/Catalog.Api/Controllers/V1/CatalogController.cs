using Catalog.Api.Entities;
using Catalog.Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.Api.Controllers
{
    [Route("Api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class CatalogController : Controller
    {

        public readonly IProductRepository _repository;
        public readonly ILogger<CatalogController> _logger;
        public CatalogController(IProductRepository repository,ILogger<CatalogController> logger)
        {
            this._repository = repository;
            this._logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var result = await _repository.GetProducts();
            return Ok(result);
        }

        [HttpGet("{id:length(24)}",Name ="GetProduct")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var result = await _repository.GetProductById(id);
            if((result is null))
            {
                _logger.LogError($"Product with id {id} , not found");
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("[action]/{categoryName}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductByCategory(string categoryName)
        {
            var result = await _repository.GetProductByCategory(categoryName);
            if ((result is null))
            {
                _logger.LogError($"Product with category {categoryName} , not found");
                return NotFound();
            }
            return Ok(result);
        }


        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody]Product product)
        {
            await _repository.Create(product);
           
            return CreatedAtRoute("GetProduct", new { id = product.Id},product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _repository.Update(product));
        }


     /*   [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            return Ok(await _repository.Delete(id));
        }*/
    }
}
