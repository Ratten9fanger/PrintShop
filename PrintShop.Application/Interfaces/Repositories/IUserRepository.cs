using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<(Guid? Guid, string? Error)> Create(User user);
        Task<(string? error, User? user)> GetUserByName(string email);
    }
}