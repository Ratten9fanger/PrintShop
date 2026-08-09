using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces.Services;

namespace PrintShop.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            //var userId = HttpContext.User.Claims.
            //var cart = _cartService.GetCart();

            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddProduct([FromBody] CartPositionRequest request)
        {

            //URL должен создавать запись в таблице CartPositions

            //var userId = httpcontext.user.getuserId - КРИТЕРИЙ ПРОВЕРКИ АВТОРИЗИРОВАН ЛИ ПОЛЬЗОВАТЕЛЬ
            var userId = HttpContext.User.FindFirst("userId");

            //Guid.Parse(userIdClaim.Value);

            if (userId == null)
            {
                var tempId = Guid.NewGuid();
                Response.Cookies.Append("cartId", tempId.ToString()); //можно защитить куку
                _cartService.AddPositionToCart(userId, tempId, request.productId, request.quantity);
            }
            //    cartId = HttpContext.Request.Cookies["cartId"].ToString();
            //else{
            //  userId = HttpContext.User.Claims.FindFirst["userid"]
            //  cartId = HttpContext.User.Claims.FindFirst["cartId"]
            //}

            //cartService.AddPositionToCart(userId, cartId, request.ProductId, request.Quantity);

            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct()
        {
            //URL должен обновлять кол-во товаров в таблице CartItems

            return BadRequest();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct()
        {
            //URL должен удалять товар в таблице CartItems
            return BadRequest();
        }
    }
}
