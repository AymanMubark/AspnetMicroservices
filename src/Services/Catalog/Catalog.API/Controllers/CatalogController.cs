using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _repository.GetProducts());
        }

        [HttpGet("{id}",Name = "GetProductsById")]
        public async Task<ActionResult<Product>> GetProductsById(string id)
        {
            Product? product = await _repository.GetProductById(id);
            if (product == null)
            {
                _logger.LogError($"Product with id: {id} not found.");
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet("GetProductByCategory/{category}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {  
            return Ok(await _repository.GetProductByCategory(category));
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await _repository.CreateProduct(product);
            return CreatedAtRoute("GetProductsById", new {id = product.Id},product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateProduct(string id,[FromBody] Product product)
        {
            return Ok(await _repository.UpdateProduct(product));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteProduct(string id)
        {
            return Ok(await _repository.DeleteProduct(id));
        }
    }
}
