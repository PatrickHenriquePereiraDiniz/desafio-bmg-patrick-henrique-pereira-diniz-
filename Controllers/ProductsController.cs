using Microsoft.AspNetCore.Mvc;
using DesafioBMG.DTOs;
using DesafioBMG.Services;
using Microsoft.AspNetCore.Authorization;

namespace DesafioBMG.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _service;
        public ProductsController(ProductService service) => _service = service;

        [HttpGet]
        [Authorize]
        public IActionResult GetAll() => Ok(_service.GetAllProducts());

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] CreateProductRequest request)
        {
            var product = _service.CreateProduct(request.Name, request.Price, request.Stock);
            return Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid id, [FromBody] CreateProductRequest request)
        {
            var product = _service.GetAllProducts().FirstOrDefault(p => p.Id == id);
            if (product == null) return NotFound();
            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;
            _service.UpdateProduct(product);
            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid id)
        {
            _service.DeleteProduct(id);
            return NoContent();
        }
    }
}
