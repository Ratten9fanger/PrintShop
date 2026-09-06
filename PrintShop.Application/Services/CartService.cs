using Microsoft.Extensions.Logging;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.Application.Interfaces.Services;
using PrintShop.Domain.Models;

namespace PrintShop.Application.Services
{
    public class CartService(IProductRepository productRepository, ICartRedisRepository redisRepository, IOrderRepository orderRepository, ILogger<CartService> logger) : ICartService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ICartRedisRepository _redisRepository = redisRepository;
        private readonly IProductRepository _productRepository = productRepository;
        private readonly ILogger<CartService> _logger = logger;

        public async Task<Cart> GetCart(Guid userId)
        {
            return await _redisRepository.GetAsync(userId);
        }

        public async Task<(string? Error, Guid? PositionId)> AddPositionToCart(Guid userId, Guid productId, int quantity)
        {
            var productResult = await _productRepository.GetById(productId);

            if (productResult.Error != null)
                return (productResult.Error, null);

            var product = productResult.Product!;

            if (quantity > product.StockQuantity)
            {
                _logger.LogWarning("При покупке товара у {userId} не хватило желаемого количества для товара {product}", userId, product);
                return ($"We have {product.StockQuantity} of this product right now", null);
            }

            _logger.LogInformation("Продукт получен {product}", product);

            var cart = await _redisRepository.GetAsync(userId);            

            var cartResult = cart.AddOrUpdatePosition(productId, quantity, product.Price);

            if (cartResult.Error != null)
                return (cartResult.Error, null);

            if (cartResult.isNew == false && product.StockQuantity < 1)
            {
                _logger.LogWarning("При повторном добавлении товара {product} не хватило количества для инкрементации", product);
                return ("We don't have this product in stock for the incrementation", null);
            }

            var guid = await _redisRepository.SaveAsync(cart);

            return (null, guid);
        }

        public async Task<(string? Error, Guid? OrderId)> CreateOrder(Guid userId)
        {
            var cart = await _redisRepository.GetAsync(userId);

            if (cart is null) 
                return ("Your cart is empty", null);

            var orderResult = await _orderRepository.CreateOrder(cart);

            if (orderResult.Error != null) 
                return (orderResult.Error, null);

            var id = await _redisRepository.Clear(userId);

            return (null, orderResult.OrderId);
        }
    }
}
