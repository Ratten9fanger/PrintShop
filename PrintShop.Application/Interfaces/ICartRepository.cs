using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<(string? Error, Guid? PositionId)> AddPositionAsync(Cart cart, Guid productId, int quantity, decimal price);
        Task<Cart> GetCartById(Guid cartId);
    }
}