using Microsoft.EntityFrameworkCore;
using PrintShop.Application.Interfaces;
using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;

namespace PrintShop.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PrintShopDbContext _context;

        public CategoryRepository(PrintShopDbContext context)
        {
            _context = context;
        }

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

            return (null, category.Id);
        }

        public async Task<(string? Error, Guid? Id)> DeleteAsync(Guid id)
        {
            // Проверяем, есть ли продукты
            var hasProducts = await _context.Products.AnyAsync(p => p.CategoryId == id);
            if (hasProducts)
                return ("This category has products", null);

            await _context.Categories
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync();

            return (null, id);
        }
    }
}
