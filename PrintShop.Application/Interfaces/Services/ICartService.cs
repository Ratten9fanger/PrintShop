using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces.Services
{
    public interface ICartService
    {
        Task<(string? Error, Guid? PositionId)> AddPositionToCart(Guid? userId, Guid cartId, Guid productId, int quantity);
        Task<Cart> GetCart(Guid cartId);
    }
}