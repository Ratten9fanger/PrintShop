using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<(string? Error, Product? Product)> GetById(Guid productId);
    }
}