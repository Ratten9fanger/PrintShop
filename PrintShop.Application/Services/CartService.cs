using PrintShop.Application.Interfaces.Repositories;
using PrintShop.Application.Interfaces.Services;
using PrintShop.Domain.Models;

namespace PrintShop.Application.Services
{
    public class CartService : ICartService
    {
        //private readonly ICartRepository _cartRepository;
        private readonly ICartRedisRepository _redisRepository;
        private readonly IProductRepository _productRepository;

        public CartService(IProductRepository productRepository, ICartRedisRepository redisRepository)
        {
            //_cartRepository = cartRepository;
            _productRepository = productRepository;
            _redisRepository = redisRepository;
        }

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
                return ($"We have {product.StockQuantity} of this product right now", null);


            //убрать
            Console.WriteLine($"Продукт получен:{product.Title}");


            var cart = await _redisRepository.GetAsync(userId);            

            var cartResult = cart.AddOrUpdatePosition(productId, quantity, product.Price);

            if (cartResult.Error != null)
                return (cartResult.Error, null);

            if (cartResult.isNew == false && product.StockQuantity < 1)
            {
                return ("This position already exists in cart and we don't have this product in stock for the incrementation", null);
            }

            var guid = await _redisRepository.SaveAsync(cart);

            return (null, guid);
        }

        public async Task<(string? Error, Guid? OrderId)> CreateOrder(Guid userId)
        {
            var cart = _redisRepository.GetAsync(userId);



            // найти товар в доменной корзине
            // удалить из редис и вернуть guid
        }
    }
}
