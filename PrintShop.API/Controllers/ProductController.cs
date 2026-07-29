using Microsoft.AspNetCore.Mvc;
using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces;

namespace PrintShop.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var products = await _productService.GetProducts();

            return Ok(products);
        }

        [HttpPost]
        public async Task<Guid> Create(PoductRequest productRequest)
        {

        }
    }
}
