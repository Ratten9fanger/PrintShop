using PrintShop.Application.Interfaces;
using PrintShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _productRepository.GetAll();
        }

        public async Task<Guid> CreateProduct(Product product)
        {
            return await _productRepository.Create(product);
        }

        public async Task<Guid> DeleteProduct(Guid id)
        {
            return await _productRepository.Delete(id);
        }
    }
}
