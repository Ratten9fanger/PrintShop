using PrintShop.Application.Interfaces.Repositories;
using PrintShop.Application.Interfaces.Services;
using PrintShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<Cart> GetCart(Guid cartId)
        {
            //варианта два либо мы карт айди получаем из таблицы зареганных корзин в табилце Carts
            //Либо берем из куки и ищем по ней
            //подменил - потерял
            var cart = await _cartRepository.GetCartById(cartId);

            return cart;
        }

        //(null, 123, 1, 55); - cartId из куки

        //(234, 567, 1, 55); - cartId из БД
        public async Task<(string?, Guid?)> AddPositionToCart(Guid? userId, Guid cartId, Guid productId, int quantity)
        {
            var productResult = await _productRepository.GetById(productId);

            if (productResult.Error != null)
                return (productResult.Error, null);

            var product = productResult.Product!;

            if (quantity > product.StockQuantity)
                return ($"We have {product.StockQuantity} of this product right now", null);

            var cart = await _cartRepository.GetCartById(cartId);

            var result = await _cartRepository.AddPositionAsync(cart, productId, quantity, product.Price);

            if (result.Error != null) return (result.Error, null);

            return (null, result.PositionId);

        }

    }
}
