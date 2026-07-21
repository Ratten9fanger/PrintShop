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

        public async Task<Cart> GetCartById(Guid cartId)
        {
            var cartPositionEntities = await _context.CartPositions.Where(x => x.CartId == cartId).ToListAsync();
            
            if (cartPositionEntities.Any())
            {
                var domainCartPositions = cartPositionEntities.Select(x => CartPosition.Create(x.Id, x.CartId, x.ProductId, x.Quantity, x.PriceAtMoment).CartPosition).ToList();
                var domainCart = Cart.Create(cartId, domainCartPositions).Cart;
            }

            return Cart.Create(cartId, null).Cart; 
        }



        public async Task<Guid> GetCartIdByUserId(Guid userId)
        {
            var cartEntity = await _context.Carts.Where(x => x.UserId == userId).FirstOrDefaultAsync();

            return cartEntity.Id;
        }

        public async Task<Cart> GetCartWithPositions(Guid userId)
        {
            //проводим поиск и возвращаем доменный обьект корзины с позициями
            var cartPositions = await _context.Carts.Where(x => x.UserId == userId).FirstOrDefaultAsync();

            return cartEntity.Id;
        }
    }
}
