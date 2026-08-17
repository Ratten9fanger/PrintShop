using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces.Repositories
{
    public interface ICartRepository
    {
        Task<(string? Error, Guid? PositionId)> AddPositionAsync(Cart cart, Guid productId, int quantity, decimal price);
        Task<Guid> CreateAnonimousCartAsync(Guid Id);
        Task<Cart> GetCartById(Guid cartId);
        Task<Guid> GetIdByUserId(Guid userId);
    }
}