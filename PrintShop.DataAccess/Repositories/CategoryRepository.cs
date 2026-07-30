using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.DataAccess.Repositories
{
    public class CategoryRepository
    {
        private readonly PrintShopDbContext _context;

        public CategoryRepository(PrintShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            var entities = await _context.Categories.ToListAsync();

            // Маппинг Entity -> Domain
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

        public async Task UpdateAsync(Category category)
        {
            // Используем ExecuteUpdateAsync для быстрого обновления одного поля, 
            // или классический подход. Здесь классический для простоты.
            var entity = await _context.Categories.FindAsync(category.Id);
            if (entity != null)
            {
                entity.Name = category.Name;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            // ВАЖНО: Если в категории есть продукты, БД выдаст ошибку Foreign Key.
            // В идеале нужно проверить это заранее или ловить DbUpdateException.
            await _context.Categories
                .Where(c => c.Id == id)
                .ExecuteDeleteAsync(); // EF Core 7+ быстрое удаление
        }
    }
}
