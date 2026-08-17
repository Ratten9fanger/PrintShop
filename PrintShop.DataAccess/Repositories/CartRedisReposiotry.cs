using PrintShop.Application.Dtos;
using PrintShop.Domain.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace PrintShop.DataAccess.Repositories
{
    public class CartRedisReposiotry
    {
        private readonly IConnectionMultiplexer _redis;

        private static readonly TimeSpan defaultTTL = TimeSpan.FromHours(24);

        public CartRedisReposiotry(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<Guid> SaveAsync(Cart cart)
        {
            var db = _redis.GetDatabase();

            var cartDto = new CartDto(
                cart.Id,
                cart.Positions
                    .Select(p => new CartPositionDto(p.Id, p.ProductId, p.Quantity, p.PriceAtMoment))
                    .ToList()
            );

            string cartJson = JsonSerializer.Serialize(cartDto);

            await db.StringSetAsync($"cart:{cart.Id}", cartJson, defaultTTL);

            return cart.Id;
        }

        public async Task<Cart?> GetAsync(Guid cartId)
        {
            var db = _redis.GetDatabase();

            string? json = await db.StringGetAsync($"cart:{cartId}");
            if (json == null) return null;

            var cartDto = JsonSerializer.Deserialize<CartDto>(json);
            if (cartDto == null) return null;

            var positionsDomain = cartDto.Positions
                .Select(p => CartPosition.Create(p.Id, cartId, p.ProductId, p.Quantity, p.PriceAtMoment).CartPosition)
                .OfType<CartPosition>()
                .ToList();

            return Cart.Create(cartId, positionsDomain).Cart;
        }


    }
}
