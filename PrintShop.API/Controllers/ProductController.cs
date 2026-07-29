using Microsoft.AspNetCore.Mvc;
using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces;
using PrintShop.Domain.Models;
using System.Text;

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
        public async Task<ActionResult<List<Product>> Get()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> Create(PoductRequest productRequest)
        {
            var domainProduct = Product.CreateProduct(
                Guid.NewGuid(),
                productRequest.Title,
                productRequest.Description,
                productRequest.Price,
                productRequest.StockQuantity,
                productRequest.CategoryId);
            
            if (domainProduct.error != null)
                return BadRequest(domainProduct.error);

           var guid = _productService.
        }

        [HttpUpdate]
        public async Task<ActionResult> Update(PoductRequest productRequest)
        {
            var domainProduct = Product.CreateProduct(
                Guid.NewGuid(),
                productRequest.Title,
                productRequest.Description,
                productRequest.Price,
                productRequest.StockQuantity,
                productRequest.CategoryId);

            if (domainProduct.error != null)
                return BadRequest(domainProduct.error);

            var guid = _productService.
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid id)
        {
            var guid = _productService.DeleteProduct(id);

            return guid;
        }
    }
}
