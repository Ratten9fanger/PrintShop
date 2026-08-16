using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;

namespace PrintShop.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        private readonly PrintShopDbContext _context;

        public ProductRepository(PrintShopDbContext context, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")!;
            _context = context;
        }

        public async Task<bool> IsEnough(Guid productId)
        {
            // 1. Создаем соединение. using гарантирует, что оно вернется в пул соединений сразу после использования
            await using var connection = new NpgsqlConnection(_connectionString);

            const string sql = @"
            SELECT EXISTS(
                SELECT 1 
                FROM ""Products"" 
                WHERE ""Id"" = @ProductId AND ""StockQuantity"" > 1
            )";

            bool isEnough = await connection.ExecuteScalarAsync<bool>(sql, new { ProductId = productId });

            return isEnough;
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

        public async Task<(string? error, Guid? id)> Delete(Guid id)
        {
            var result = await _context.Products.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (result == 0)
                return ("Product not found", null);

            return (null, id);
        }

        public async Task<(string? error, Guid? id)> Update(Product product)
        {
            var exists = await _context.Products.AnyAsync(x => x.Id == product.Id);
            if (!exists)
                return ("Product not found", null);

            var rowsAffected = await _context.Products
                .Where(x => x.Id == product.Id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(b => b.Title, product.Title)
                    .SetProperty(b => b.Description, product.Description)
                    .SetProperty(b => b.Price, product.Price)
                    .SetProperty(b => b.PriceAtMoment, product.Price)
                    .SetProperty(b => b.StockQuantity, product.StockQuantity)
                    .SetProperty(b => b.CategoryId, product.CategoryId)
                );

            return (null, product.Id);
        }
    }
}
