using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;

namespace PrintShop.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PrintShopDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(PrintShopDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<(Guid? Guid, string? Error)> Create(User user)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var isUserExists = await _context.Users.AnyAsync(x => x.Email == user.Email);

                    if (isUserExists)
                        return (null, "This user is already exists");

                    var userEntity = new UserEntity
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Role = user.Role,
                        PasswordHash = user.PasswordHash
                    };

                    await _context.Users.AddAsync(userEntity);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    _logger.LogInformation("Пользователь успешно создан {user}", user);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Ошибка при создании пользователя {user} - {ex.Message}", user, ex.Message);
                    transaction.Rollback();
                    return (null, ex.Message);
                }
            }

            return (user.Id, null);
        }

        public async Task<(string? error, User? user)> GetUserByName(string email)
        {
            var userEntity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

            if (userEntity == null)
                return ("This user don't exist", null);

            var user = User.Create(userEntity.Id, userEntity.Email, userEntity.Role, userEntity.PasswordHash).user;

            return (null, user);
        }

        //public async Task<List<User>> Get()
        //{
        //    var userEntities = await _context.Users.AsNoTracking().ToListAsync();

        //    var users = userEntities.Select(x => User.Create(x.Id, x.Name, x.PasswordHash).User).ToList();

        //    return users;
        //}
    }
}
