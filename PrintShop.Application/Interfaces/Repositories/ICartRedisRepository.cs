using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces.Repositories
{
    public interface ICartRedisRepository
    {
        Task<Cart> GetAsync(Guid userId);
        Task<Guid> SaveAsync(Cart cart);
    }
}