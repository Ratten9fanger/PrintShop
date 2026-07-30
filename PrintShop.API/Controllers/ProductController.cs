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
        public async Task<ActionResult<List<Product>>> Get()
        {
            var products = await _productService.GetProducts();
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ProductRequest productRequest)
        {
            var domainProduct = Product.Create(
                Guid.NewGuid(),
                productRequest.Title,
                productRequest.Description,
                productRequest.Price,
                productRequest.StockQuantity,
                productRequest.CategoryId);

            if (domainProduct.error != null)
                return BadRequest(domainProduct.error);

            var guid = await _productService.CreateProduct(domainProduct.product);

            return Ok(guid);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] ProductRequest productRequest, Guid id)
        {
            var domainProduct = Product.Create(
                id,
                productRequest.Title,
                productRequest.Description,
                productRequest.Price,
                productRequest.StockQuantity,
                productRequest.CategoryId);

            if (domainProduct.error != null)
                return BadRequest(domainProduct.error);

            var result = await _productService.UpdateProduct(domainProduct.product);

            if (result.error != null)
                return BadRequest(result.error);

            return Ok(result.guid);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _productService.DeleteProduct(id);

            if (result.error != null)
                return BadRequest(result.error);

            return Ok(result.guid);
        }
    }
}
