using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces;
using PrintShop.Domain.Models;

namespace PrintShop.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IHashService hashService;
        private readonly IJwtService jwtService;

        public UserService(
            IUserRepository userRepository,
            IHashService hashService,
            IJwtService jwtService)
        {
            this.userRepository = userRepository;
            this.hashService = hashService;
            this.jwtService = jwtService;
        }

        public async Task<(Guid? id, string? error)> CreateUser(RegisterRequest registerRequest)
        {
            var hash = hashService.Hash(registerRequest.Password);

            var domainUser = User.Create(
                Guid.NewGuid(),
                registerRequest.Email,
                "User",
                hash
            );

            if (!String.IsNullOrWhiteSpace(domainUser.error))
                return (null, domainUser.error);

            var result = await userRepository.Create(domainUser.user!);

            if (!String.IsNullOrWhiteSpace(result.Error))
                return (null, result.Error);

            return (result.Guid, null);
        }

        public async Task<(string token, string error)> Login(string name, string password)
        {
            var user = await userRepository.GetUserByName(name); //null || user

            if (user == null)
                return (string.Empty, "This user doesn't exist");

            if (!hashService.Verify(password, user.PasswordHash))
                return (string.Empty, "This user doesn't exist");

            var token = jwtService.CreateToken(user);

            return (token, error);
        }

        public async Task<List<User>> GetAllUsers() => await userRepository.Get();
        
    }
}
