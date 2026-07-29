using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces
{
    public interface ICartService
    {
        Task<(string?, Guid?)> AddPositionToCart(Guid? userId, Guid cartId, Guid productId, int quantity);
        Task<Cart> GetCart(Guid cartId);
    }
}