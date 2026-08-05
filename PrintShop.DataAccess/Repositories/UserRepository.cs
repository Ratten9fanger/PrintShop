using Microsoft.EntityFrameworkCore;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;
using System.Xml.Linq;

namespace PrintShop.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PrintShopDbContext _context;

        public UserRepository(PrintShopDbContext context)
        {
            _context = context;
        }

        public async Task<(Guid? Guid, string? Error)> Create(User user)
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

            return (user.Id, null);
        }


        //public async Task<User?> GetUserByName(string name)
        //{
        //    var userEntity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);

        //    if (userEntity is null)
        //        return null;

        //    var user = User.Create(userEntity.Id, userEntity.Name, userEntity.PasswordHash).User;

        //    return user;
        //}

        //public async Task<List<User>> Get()
        //{
        //    var userEntities = await _context.Users.AsNoTracking().ToListAsync();

        //    var users = userEntities.Select(x => User.Create(x.Id, x.Name, x.PasswordHash).User).ToList();

        //    return users;
        //}
    }
}
