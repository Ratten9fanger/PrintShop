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

                return domainCart;
            }

            return Cart.Create(cartId, null).Cart; 
        }

        public async Task<Guid> SaveAsync(Cart cart)
        {
            // 1. Пытаемся найти запись корзины в БД
            var existingCartEntity = await _context.Carts
                .Include(c => c.CartPositions)
                .FirstOrDefaultAsync(c => c.Id == cart.Id);

            if (existingCartEntity == null)
            {
                // СЦЕНАРИЙ А: Корзины в БД еще нет (первый товар анонима).
                // Создаем новую запись CartEntity и все позиции внутри неё.
                var newCartEntity = new CartEntity
                {
                    Id = cart.Id,
                    UserId = null, // Или передай userId, если он есть
                    CartPositions = cart.Positions.Select(p => new CartPositionEntity
                    {
                        Id = p.Id,
                        ProductId = p.ProductId,
                        Quantity = p.Quantity,
                        PriceAtMoment = p.PriceAtMoment,
                        AddedAt = DateTime.UtcNow
                    }).ToList()
                };

                _context.Carts.Add(newCartEntity); // EF Core сам сделает INSERT и для Cart, и для Positions
            }
            else
            {
                // СЦЕНАРИЙ Б: Корзина уже есть в БД. Обновляем её.

                //// 1. Если аноним зарегистрировался, привязываем UserId
                //if (existingCartEntity.UserId == null && /* тут можно передать userId из домена, если добавишь свойство */)
                //{
                //    // existingCartEntity.UserId = cart.UserId; 
                //}

                // 2. Синхронизируем позиции
                var domainPositionIds = cart.Positions.Select(p => p.Id).ToHashSet();

                // Удаляем из БД те позиции, которых нет в домене (на случай удаления из корзины)
                var positionsToRemove = existingCartEntity.CartPositions
                    .Where(p => !domainPositionIds.Contains(p.Id))
                    .ToList();
                _context.CartPositions.RemoveRange(positionsToRemove);

                // Добавляем или обновляем оставшиеся
                foreach (var domainPos in cart.Positions)
                {
                    var existingPos = existingCartEntity.CartPositions.FirstOrDefault(p => p.Id == domainPos.Id);

                    if (existingPos == null)
                    {
                        // Это НОВАЯ позиция (result.IsNew == true)
                        existingCartEntity.CartPositions.Add(new CartPositionEntity
                        {
                            Id = domainPos.Id,
                            ProductId = domainPos.ProductId,
                            Quantity = domainPos.Quantity,
                            PriceAtMoment = domainPos.PriceAtMoment
                        });
                    }
                    else
                    {
                        // Это СУЩЕСТВУЮЩАЯ позиция, просто меняем количество
                        existingPos.Quantity = domainPos.Quantity;
                    }
                }
            }

            // 3. Единая команда сохранения всех изменений (INSERT / UPDATE / DELETE)
            await _context.SaveChangesAsync();
        }


        public async Task<bool> CreateNew(Guid cartId, Guid userId)
        {
            var existingCart = await _context.Carts.Where(x => x.Id == cartId).FirstOrDefaultAsync();

            if (existingCart != null)
            {
                return false;
            }

            await _context.AddAsync(new CartEntity { Id = cartId, UserId = userId });

            return true;
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
