using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;

namespace PrintShop.DataAccess.Repositories
{
    public class CategoryRepository(PrintShopDbContext context, ILogger<CategoryRepository> logger) : ICategoryRepository
    {
        private readonly PrintShopDbContext _context = context;
        private readonly ILogger<CategoryRepository> _logger = logger;

        public async Task<List<Category>> GetAllAsync()
        {
            var entities = await _context.Categories.AsNoTracking().ToListAsync();

            return entities.Select(e => Category.Create(e.Id, e.Name).Category!).ToList();
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null) return null;

            return Category.Create(entity.Id, entity.Name).Category;
        }

        public async Task AddAsync(Category category)
        {
            var entity = new CategoryEntity
            {
                Id = category.Id,
                Name = category.Name
            };

            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Создана новая категория {entity}", entity);
        }

        public async Task<(string? Error, Guid? Id)> UpdateAsync(Category category)
        {
            var entity = await _context.Categories.FindAsync(category.Id);
            if (entity != null)
            {
                entity.Name = category.Name;
                await _context.SaveChangesAsync();
            }
            else return ("Not found category to update", null);

            _logger.LogInformation("Обновлена категория {entity}", entity);

            return (null, category.Id);
        }

        public async Task<(string? Error, Guid? Id)> DeleteAsync(Guid id)
        {
            var category = await _context.Products.FindAsync(id);

            if (await _context.Products.AnyAsync(p => p.CategoryId == id))
            {
                _logger.LogInformation("Недуачная попытка удаления категории {category} - в ней есть записи", category);
                return ("This category has products", null);
            }

            await _context.Categories
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();

            _logger.LogInformation("Удалена категория {category}", category);

            return (null, id);
        }
    }
}
