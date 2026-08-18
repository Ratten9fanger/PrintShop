using PrintShop.Application.Interfaces.Repositories;
using PrintShop.Application.Interfaces.Services;
using PrintShop.Domain.Models;

namespace PrintShop.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartRedisRepository _redisRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository ICartRedisRepository redisRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _redisRepository = redisRepository;
        }

        public async Task<Cart> GetCart(Guid cartId)
        {
            //варианта два либо мы карт айди получаем из таблицы зареганных корзин в табилце Carts
            //Либо берем из куки и ищем по ней
            //подменил - потерял
            var cart = await _cartRepository.GetCartById(cartId);

            return cart;
        }

        public async Task<(string? Error, Guid? PositionId)> AddPositionToCart(Guid userId, Guid productId, int quantity)
        {
            var productResult = await _productRepository.GetById(productId);

            if (productResult.Error != null)
                return (productResult.Error, null);

            var product = productResult.Product!;

            if (quantity > product.StockQuantity)
                return ($"We have {product.StockQuantity} of this product right now", null);



            //убрать
            Console.WriteLine($"Продукт получен:{product.Title}");


            Cart cart = await _redisRepository.GetAsync(userId);

            var cartResult = cart.AddOrUpdatePosition(productId, quantity, product.Price);

            if (cartResult.Error != null)
                return (cartResult.Error, null);

            if (cartResult.isNew)
            {
                var guid = await _redisRepository.SaveAsync(cartResult);

                return (guid, null);
            }

            return (null, await _redisRepository.IncrementPosition(userId, productId));
        }

    }
}
