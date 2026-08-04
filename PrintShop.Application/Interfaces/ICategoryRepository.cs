using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
        Task<(string? Error, Guid? Id)> DeleteAsync(Guid id);
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
        Task<(string? Error, Guid? Id)> UpdateAsync(Category category);
    }
}