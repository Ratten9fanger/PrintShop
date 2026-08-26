using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<(string? Error, Guid? PositionId)> AddPositionToCart(Guid userId, Guid productId, int quantity);
        Task<(string? Error, Guid? OrderId)> CreateOrder(Guid userId);
        Task<Cart> GetCart(Guid userId);
    }
}