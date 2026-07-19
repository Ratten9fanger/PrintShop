using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> Get();
        Task<(int Stock, decimal Price)> GetProductInfoById(Guid productId);
        Task<bool> IsProductExists(Guid Id);
    }
}