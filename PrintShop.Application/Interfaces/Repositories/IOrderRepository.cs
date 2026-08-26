using PrintShop.Application.Dtos;
using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task<(Guid? OrderId, string? Error)> CreateOrder(Cart cart);
        Task<List<OrderDto>?> GetByUserId(Guid userId);
    }
}