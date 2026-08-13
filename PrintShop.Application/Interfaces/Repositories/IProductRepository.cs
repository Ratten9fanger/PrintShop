using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<Guid> Create(Product product);
        Task<(string? error, Guid? id)> Delete(Guid id);
        Task<List<Product>> GetAll();
        Task<(string? Error, Product? Product)> GetById(Guid productId);
        Task<bool> IsEnough(Guid productId);
        Task<(string? error, Guid? id)> Update(Product product);
    }
}