using Microsoft.EntityFrameworkCore;
using PrintShop.Application.Interfaces;
using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;

namespace PrintShop.DataAccess.Repositories
{
    public class UserRepository  
    {
        private readonly PrintShopDbContext _context;

        public UserRepository(PrintShopDbContext context)
        {
            _context = context;
        }
    }
}
