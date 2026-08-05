using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}