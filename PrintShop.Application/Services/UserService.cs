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

        //должен создать пользователя в бд
        public async Task<Guid> SignUp(User user)
        {
            var hash = hashService.Hash(user.PasswordHash);

            await userRepository.Create(user.Id, user.Name, hash); //переназаначить пароль и ипередать обьект?

            return user.Id; //if false - return bad request
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

        public async Task<bool> CheckUserExistence(string name) => await userRepository.CheckUserExistence(name);

        public async Task<List<User>> GetAllUsers() => await userRepository.Get();
        
    }
}
