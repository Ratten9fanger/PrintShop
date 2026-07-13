using Microsoft.EntityFrameworkCore;
using PrintShop.Application.Interfaces;
using PrintShop.Domain.Models;

namespace PrintShop.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly PrintShopDbContext _context;

        public ProductRepository(PrintShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> Get()
        {
            var productEntities = await _context.Products.AsNoTracking().ToListAsync();

            var products = productEntities
                .Select(x => Product.Create(x.Id, x.Title, x.Description, x.Price, x.StockQuantity, x.CategoryId).product)
                .ToList();

            return products;
        }

        public async Task<bool> IsProductExists(Guid Id)
        {
            var product = await _context.Products
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();

            if (product == null)
                return false;

            return true;
        }

        public async Task<int> GetStockById(Guid productId)
        {
            var productEntity = await _context.Products
                .Where(x => x.Id == productId)
                .FirstOrDefaultAsync();

            return productEntity.StockQuantity;
        }
    }
}
