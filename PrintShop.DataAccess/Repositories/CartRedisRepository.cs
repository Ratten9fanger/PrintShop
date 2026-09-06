using Microsoft.Extensions.Logging;
using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.Domain.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace PrintShop.DataAccess.Repositories
{
    public class CartRedisRepository : ICartRedisRepository
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<CartRedisRepository> _logger;

        private static readonly TimeSpan defaultTTL = TimeSpan.FromHours(24);

        public CartRedisRepository(IConnectionMultiplexer redis, ILogger<CartRedisRepository> logger)
        {
            _redis = redis;
            _logger = logger;
        }



        public async Task<Guid> SaveAsync(Cart cart)
        {
            var db = _redis.GetDatabase();

            _logger.LogError("Ошибка базы данных: {db}", db);

            var cartDto = new CartDto(
                cart.UserId,
                cart.Positions
                    .Select(p => new CartPositionDto(p.Id, p.ProductId, p.Quantity, p.PriceAtMoment))
                    .ToList()
            );

            string cartJson = JsonSerializer.Serialize(cartDto);

            await db.StringSetAsync($"user:{cart.UserId}", cartJson, defaultTTL);

            _logger.LogInformation("Занесено значение в redis: {cartJson}", cartJson);

            return cart.UserId;
        }

        public async Task<Cart> GetAsync(Guid userId)
        {
            var db = _redis.GetDatabase();

            string? json = await db.StringGetAsync($"user:{userId.ToString()}");

            _logger.LogWarning("Получено значение redis: {json}", json);

            if (json == null) return Cart.Create(userId, null).Cart!;

            var cartDto = JsonSerializer.Deserialize<CartDto>(json);
            if (cartDto == null) return Cart.Create(userId, null).Cart!;

            var positionsDomain = cartDto.Positions
                .Select(p => CartPosition.Create(p.Id, p.ProductId, p.Quantity, p.PriceAtMoment).CartPosition)
                .OfType<CartPosition>()
                .ToList();

            return Cart.Create(userId, positionsDomain).Cart!;
        }

        public async Task<Guid> Clear(Guid userId)
        {
            var db = _redis.GetDatabase();

            var key = $"user:{userId.ToString()}";

            var id = await db.StringDeleteAsync(key, ValueCondition.Always);

            _logger.LogInformation($"Корзина удалена: {id}", id);

            return userId;
        }
    }
}
