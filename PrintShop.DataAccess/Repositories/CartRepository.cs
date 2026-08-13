using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;

namespace PrintShop.DataAccess.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly PrintShopDbContext _context;
        private readonly IProductRepository _productRepository;

        public CartRepository(PrintShopDbContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }

        //создание пустой доменной корзины если в ней нет позиций
        public async Task<Cart> GetCartById(Guid cartId)
        {
            var cartPositionEntities = await _context.CartPositions.Where(x => x.CartId == cartId).ToListAsync();

            if (cartPositionEntities.Any())
            {
                var domainCartPositions = cartPositionEntities
                    .Select(x => CartPosition.Create(x.Id, x.CartId, x.ProductId, x.Quantity, x.PriceAtMoment).CartPosition)
                    .OfType<CartPosition>()
                    .ToList();

                var domainCart = Cart.Create(cartId, domainCartPositions).Cart;

                return domainCart;
            }

            return Cart.Create(cartId, null).Cart;
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

            // 3. Если позиция уже есть — обновляем количество (мы должны ее найти)
            var existingPositionEntity = await _context.CartPositions
                .FirstOrDefaultAsync(x => x.CartId == cart.Id && x.ProductId == productId);

            if (existingPositionEntity == null)
                return ("Position not found", null);

            if (await _productRepository.IsEnough(productId))
            {
                existingPositionEntity.Quantity++;
                await _context.SaveChangesAsync();
            }
            else
            {
                return ("We don't have this product now to increase it quantity in your cart", null);
            }
               
            return (null, existingPositionEntity.Id);
        }

        public async Task<Guid> GetIdByUserId(Guid userId)
        {
            var cartId = await _context.Carts
                .Where(x => x.UserId == userId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            return cartId;
        }

    }
}
