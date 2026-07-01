using Microsoft.EntityFrameworkCore;
using PrintShop.DataAccess.Entities;

namespace PrintShop.DataAccess.Repositories
{
    public class ProductRepository
    {
        private readonly PrintShopDbContext _context;

        public ProductRepository(PrintShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductEntity>> Get()
        {
            var productEntities = await _context.Products.AsNoTracking().ToListAsync();

            //mapping

            return productEntities;
        }

        public async Task<bool> IsProductExists(Guid Id)
        {
            var product = await _context.Products.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (product == null)
                return false;

            return true;
        }

        public async Task<int> GetStockById(Guid productId)
        {
            var product = await _context.Products
                .Where(x => x.Id == productId)
                .FirstOrDefaultAsync();

            return product.StockQuantity;
        }
    }
}
