using PrintShop.Application.Dtos;
using PrintShop.Domain.Models;

namespace PrintShop.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<(Guid? id, string? error)> CreateUser(RegisterRequest registerRequest);
        //Task<List<User>> GetAllUsers();
        Task<(string? token, string? error)> Login(LoginRequest loginRequest);
    }
}