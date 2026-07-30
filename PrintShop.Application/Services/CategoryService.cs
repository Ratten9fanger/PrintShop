using PrintShop.Application.Dtos;
using PrintShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.Application.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<(string? Error, List<CategoryResponse>? Categories)> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var response = categories.Select(c => new CategoryResponse(c.Id, c.Name)).ToList();
            return (null, response);
        }

        public async Task<(string? Error, Guid? Id)> CreateAsync(string name)
        {
            var newId = Guid.NewGuid();
            var domainResult = Category.Create(newId, name);

            if (domainResult.Error != null)
                return (domainResult.Error, null);

            // 2. Сохранение через репозиторий
            await _categoryRepository.AddAsync(domainResult.Category!);
            return (null, newId);
        }

        public async Task<(string? Error, Guid? Id)> UpdateAsync(Guid id, string name)
        {
            // 1. Проверяем, существует ли категория
            var existing = await _categoryRepository.GetByIdAsync(id);
            if (existing == null)
                return ("Категория не найдена", null);

            // 2. Валидация новых данных через доменную модель
            var domainResult = Category.Create(id, name);
            if (domainResult.Error != null)
                return (domainResult.Error, null);

            // 3. Обновление
            await _categoryRepository.UpdateAsync(domainResult.Category!);
            return (null, id);
        }

        public async Task<(string? Error)> DeleteAsync(Guid id)
        {
            var existing = await _categoryRepository.GetByIdAsync(id);
            if (existing == null)
                return ("Категория не найдена");

            try
            {
                await _categoryRepository.DeleteAsync(id);
                return (null);
            }
            catch (Exception) // Ловим ошибку, если в категории есть продукты (Foreign Key constraint)
            {
                return ("Невозможно удалить категорию, так как в ней есть товары");
            }
        }
    }
}
