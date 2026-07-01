using Microsoft.EntityFrameworkCore;
using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.DataAccess.Repositories
{
    public class CartRepository
    {
        private readonly PrintShopDbContext _context;

        public CartRepository(PrintShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartPositionEntity>> GetByCartId(Guid cartId)
        {
            var cartPositions = await _context.CartPositions.Where(x => x.CartId == cartId).ToListAsync();

            return cartPositions;
        }

        public async Task<Guid> GetCartIdByUserId(Guid userId)
        {
            var cartEntity = await _context.Carts.Where(x => x.UserId == userId).FirstOrDefaultAsync();

            return cartEntity.Id;
        }


    }
}
