using Microsoft.Extensions.Logging;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.Application.Interfaces.Services;
using PrintShop.Domain.Models;

namespace PrintShop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<List<Product>> GetProducts(CancellationToken cancellationToken)
        {
            await Task.Delay(5000);
            return await _productRepository.GetAll(cancellationToken);
        }

        public async Task<Guid> CreateProduct(Product product)
        {
            return await _productRepository.Create(product);
        }

        public async Task<(string? error, Guid? guid)> UpdateProduct(Product product)
        {
            var result = await _productRepository.Update(product);

            if (result.error != null)
                return (result.error, null);

            return (null, result.id);
        }

        public async Task<(string? error, Guid? guid)> DeleteProduct(Guid id)
        {
            var result = await _productRepository.Delete(id);

            if (result.error != null)
                return (result.error, null);

            return (null, id);
        }
    }
}
