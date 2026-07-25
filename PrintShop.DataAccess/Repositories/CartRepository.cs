using Microsoft.EntityFrameworkCore;
using PrintShop.DataAccess.Configurations;
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
                var domainCartPositions = cartPositionEntities.Select(x => CartPosition.Create(x.Id, x.CartId, x.ProductId, x.Quantity, x.PriceAtMoment).CartPosition)
                    .OfType<CartPosition>()
                    .ToList();

                var domainCart = Cart.Create(cartId, domainCartPositions).Cart;

                return domainCart!;
            }

            return Cart.Create(cartId, null).Cart!;
        }

        public async Task<(string? Error, Guid? PositionId)> AddPositionAsync(Cart cart, Guid productId, int quantity, decimal price)
        {
            // 1. Проверяем, нужно ли создавать новую позицию (доменная модель знает это лучше)
            var (error, isNew) = cart.AddOrUpdatePosition(productId, quantity, price);
            if (error != null) return (error, null);

            // 2. Если позиция новая — просто добавляем её в БД
            if (isNew)
            {
                var positionEntity = new CartPositionEntity
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    PriceAtMoment = price,
                    AddedAt = DateTime.UtcNow
                };

                await _context.CartPositions.AddAsync(positionEntity);
                await _context.SaveChangesAsync();

                return (null, positionEntity.Id);
            }

            // 3. Если позиция уже есть — обновляем количество
            var existingPosition = await _context.CartPositions
                .FirstOrDefaultAsync(x => x.CartId == cart.Id && x.ProductId == productId);

            if (existingPosition == null)
                return ("Position not found", null);

            existingPosition.Quantity++;
            await _context.SaveChangesAsync();

            return (null, existingPosition.Id);
        }

        public async Task<Guid> GetCartIdByUserId(Guid userId)
        {
            var cartEntity = await _context.Carts.Where(x => x.UserId == userId).FirstOrDefaultAsync();

            return cartEntity.Id;
        }

        public async Task<Guid> UpdatePosition(Cart cart)
        {
            _context.
            return guid;
        }

        public async Task<Guid> UpdatePosition(Cart cart)
        {
            
            return guid;
        }
    }
}
