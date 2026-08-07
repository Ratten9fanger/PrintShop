using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.Application.Interfaces.Services;
using PrintShop.Domain.Models;

namespace PrintShop.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;

        public UserService(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
        }

        public async Task<(Guid? id, string? error)> CreateUser(RegisterRequest registerRequest)
        {
            var hash = _passwordHasher.Hash(registerRequest.Password);

            var domainUser = User.Create(
                Guid.NewGuid(),
                registerRequest.Email,
                "User",
                hash
            );

            if (!String.IsNullOrWhiteSpace(domainUser.error))
                return (null, domainUser.error);

            var result = await _userRepository.Create(domainUser.user!);

            if (!String.IsNullOrWhiteSpace(result.Error))
                return (null, result.Error);

            return (result.Guid, null);
        }

        public async Task<(string? token, string? error)> Login(LoginRequest loginRequest)
        {
            var domainUserResult = await _userRepository.GetUserByName(loginRequest.Email);
            
            if (domainUserResult.error != null)
                return (null, "This user doesn't exist");

            if (!_passwordHasher.Verify(loginRequest.Password, domainUserResult.user.PasswordHash))
                return (null, "Paswords are not the same");

            var token = _jwtService.GenerateToken(domainUserResult.user);

            return (token, null);
        }

        //public async Task<List<User>> GetAllUsers() => await _userRepository.Get();

    }
}
