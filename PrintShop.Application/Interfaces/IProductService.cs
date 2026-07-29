using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetProducts();
    }
}