using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces.Services
{
    public interface IProductService
    {
        Task<Guid> CreateProduct(Product product);
        Task<(string? error, Guid? guid)> DeleteProduct(Guid id);
        Task<List<Product>> GetProducts();
        Task<(string? error, Guid? guid)> UpdateProduct(Product product);
    }
}