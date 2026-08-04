using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces;
using PrintShop.Domain.Models;

namespace PrintShop.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryResponse>> GetAll()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryResponse(c.Id, c.Name)).ToList();
        }

        public async Task<(string? Error, Guid? Id)> Create(string name)
        {
            var newId = Guid.NewGuid();
            var domainResult = Category.Create(newId, name);

            if (domainResult.Error != null)
                return (domainResult.Error, null);

            await _categoryRepository.AddAsync(domainResult.Category!);
            return (null, newId);
        }

        public async Task<(string? Error, Guid? Id)> Update(Guid id, string name)
        {
            var domainResult = Category.Create(id, name);
            if (domainResult.Error != null)
                return (domainResult.Error, null);

            var result = await _categoryRepository.UpdateAsync(domainResult.Category!);

            if (result.Error != null) return (result.Error, null);

            return (null, id);
        }

        public async Task<(string? Error, Guid? Id)> Delete(Guid id)
        {
            var result = await _categoryRepository.DeleteAsync(id);

            if (result.Error != null) return (result.Error, null);

            return (null, result.Id);
        }
    }
}
