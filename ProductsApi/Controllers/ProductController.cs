using Microsoft.AspNetCore.Mvc;
using ProductsApi.Repository;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
