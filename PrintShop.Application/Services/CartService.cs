using PrintShop.Application.Interfaces;
using PrintShop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.Application.Services
{
    public class CartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public Task<Guid> GetCartProducts(Guid cartId)
        {
            //варианта два либо мы карт айди получаем из таблицы зареганных корзин в табилце Carts
            //Либо берем из куки и ищем по ней
            //подменил - потерял
            //var userCartProducts = _cartRepository.GetByCartId();

            //методс сервиса по добавлению в табилцу cartitems

            //проверка наличия

            //cartRepos.

        }

        //(null, 123, 1, 55); - cartId из куки

        //(234, 567, 1, 55); - cartId из БД
        public async Task<(string?, Guid?)> AddPositionToCart(Guid? userId, Guid cartId, Guid productId, int quantity)
        {

            if (!await _productRepository.IsProductExists(productId)) 
                return ("This product doesn't exist", null);
            
            var productInfo = await _productRepository.GetProductInfoById(productId);

            if (productInfo.Stock <= 0 || quantity > productInfo.Stock) 
                return ($"We have {productInfo.Stock} of this product right now", null);

            var cart = await _cartRepository.GetCartById(cartId);

            if (userId != null)
            {
                //картАйди действительно существует т.к. лежит в клеймсах

                var result = await cart.AddOrUpdatePosition(cartId, productId, quantity);

                if (result.Error != null) return (result.Error, null);

                if (result.IsNew)
                {
                    _cartRepository.Add(cart);
                }
                else
                {
                    _cartRepository.Update(cart);
                }

                //

            }
            else
            {
                var cart 
            //    v
            //    _cartRepository.AddToCartProducts(userId, userCartId, productId, quantity);
            }

            //    //проверка наличия


            return positionId;
        }

        public Task<string> IsProductAvaliable()
        {
            //метод будет валидировать случаи остатка товара или его отсутствие
        }
    }
}
