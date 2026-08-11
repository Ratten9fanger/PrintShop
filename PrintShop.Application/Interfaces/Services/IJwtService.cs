using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces.Services
{
    public interface IJwtService
    {
        Task<string> GenerateToken(User user);
    }
}