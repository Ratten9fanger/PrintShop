using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces.Repositories
{
    public interface ICartRedisRepository
    {
        Task<Guid> Clear(Guid userId);
        Task<Cart> GetAsync(Guid userId);
        Task<Guid> SaveAsync(Cart cart);
    }
}