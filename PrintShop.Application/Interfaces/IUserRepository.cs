using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<(Guid? Guid, string? Error)> Create(User user);
    }
}