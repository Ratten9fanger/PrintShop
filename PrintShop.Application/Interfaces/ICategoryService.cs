using PrintShop.Application.Dtos;

namespace PrintShop.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<(string? Error, Guid? Id)> Create(string name);
        Task<(string? Error, Guid? Id)> Delete(Guid id);
        Task<List<CategoryResponse>> GetAll();
        Task<(string? Error, Guid? Id)> Update(Guid id, string name);
    }
}