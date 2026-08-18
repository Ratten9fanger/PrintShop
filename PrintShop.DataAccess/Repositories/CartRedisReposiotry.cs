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
                cart.UserId,
                cart.Positions
                    .Select(p => new CartPositionDto(p.Id, p.ProductId, p.Quantity, p.PriceAtMoment))
                    .ToList()
            );

            string cartJson = JsonSerializer.Serialize(cartDto);

            await db.StringSetAsync($"user:{cart.UserId}", cartJson, defaultTTL);

            return cart.Id;
        }

        public async Task<Cart?> GetAsync(Guid userId)
        {
            var db = _redis.GetDatabase();

            string? json = await db.StringGetAsync($"user:{userId}");
            if (json == null) return null;

            var cartDto = JsonSerializer.Deserialize<CartDto>(json);
            if (cartDto == null) return null;

            var positionsDomain = cartDto.Positions
                .Select(p => CartPosition.Create(p.Id, p.ProductId, p.Quantity, p.PriceAtMoment).CartPosition)
                .OfType<CartPosition>()
                .ToList();

            return Cart.Create(userId, positionsDomain).Cart;
        }

        public async Task<Guid> Increment(Guid userId, )
        {
            var db = _redis.GetDatabase();

            string? json = await db.StringGetAsync($"cart:{cartId}");

        }
    }
}
