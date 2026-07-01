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

        public CartService()
        {

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

        //(null, 123, 1, 55);

        //(234, 567, 1, 55);
        public Task<Guid> AddProductToCart(Guid? userId, Guid cartId, Guid productId, int quantity)
        {
            if (userId != null)
            {
                var userCartId = _cartRepository.GetCartIdByUserId(userId);
                _cartRepository.InsertToCart(null, cartId, productId, quantity);
            }
            //else
            //{
            //    v
            //    _cartRepository.AddToCartProducts(userId, userCartId, productId, quantity);
            //}

            //    //методс сервиса по добавлению в табилцу cartitems

            //    //проверка наличия

            //    cartRepos.

            return positionId;
        }

        public Task<string> IsProductAvaliable()
        {
            //метод будет валидировать случаи остатка товара или его отсутствие
        }
    }
}
