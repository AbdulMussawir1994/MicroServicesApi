using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.DTOs;
using ProductsApi.Repository;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("GetProductsList")]
        public async Task<ActionResult> GetProducts(CancellationToken ctx)
        {
            var response = await _productService.GetProductsAsync(ctx);

            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAll(CancellationToken ctx)
        {
            var response = await _productService.GetAllAsync(ctx);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id, CancellationToken ctx)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Product ID is required.");

            var response = await _productService.GetByIdAsync(id, ctx);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto model, CancellationToken ctx)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _productService.CreateAsync(model, ctx);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CreateProductDto model, CancellationToken ctx)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _productService.UpdateAsync(id, model, ctx);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken ctx)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest("Product ID is required.");

            var response = await _productService.DeleteAsync(id, ctx);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }
    }
}
