using Microsoft.EntityFrameworkCore;
using PrintShop.Application.Interfaces;
using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;
using System.Dynamic;

namespace PrintShop.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PrintShopDbContext _context;

        public ProductRepository(PrintShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAll()
        {
            var productEntities = await _context.Products.AsNoTracking().ToListAsync();

            var products = productEntities
                .Select(x => Product.Create(x.Id, x.Title, x.Description, x.Price, x.StockQuantity, x.CategoryId).product)
                .OfType<Product>()
                .ToList();

            return products;
        }

        public async Task<(string? Error, Product? Product)> GetById(Guid productId)
        {
            var productEntity = await _context.Products
                .Where(x => x.Id == productId)
                .FirstOrDefaultAsync();

            if (productEntity == null)
                return ("Product not found", null);

            if (productEntity.StockQuantity == 0)
                return ("We don't have this product right now", null);

            var product = Product.Create(productId,
                productEntity.Title,
                productEntity.Description,
                productEntity.Price,
                productEntity.StockQuantity,
                productEntity.CategoryId);

            if (product.error != null)
                return (product.error, null);

            return (null, product.product);
        }

        public async Task<Guid> Create(Product product)
        {
            var productEntity = new ProductEntity
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId
            };

            await _context.Products.AddAsync(productEntity);
            await _context.SaveChangesAsync();

            return productEntity.Id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Products.Where(x => x.Id == id).ExecuteDeleteAsync();

            return id;
        }
    }
}
