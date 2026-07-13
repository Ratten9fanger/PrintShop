using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> Get();
        Task<int> GetStockById(Guid productId);
        Task<bool> IsProductExists(Guid Id);
    }
}