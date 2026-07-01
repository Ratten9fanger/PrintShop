using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintShop.Application.Dtos;

namespace PrintShop.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        public CartController()
        {
                
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            //var userId = httpcontext.user.getuserId
            //cartService.GetItemsByUserId(userId);

            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddProduct([FromBody] CartItemRequest request)
        {
            //URL должен создавать запись в таблице CartItems

            //var userId = httpcontext.user.getuserId - КРИТЕРИЙ ПРОВЕРКИ АВТОРИЗИРОВАН ЛИ ПОЛЬЗОВАТЕЛЬ

            //if (userId == null)
            //    cartId = HttpContext.Request.Cookies["cartId"].ToString();
            //else{
            //  userId = HttpContext.User.Claims.FindFirst["userid"]
            //  cartId = HttpContext.User.Claims.FindFirst["cartId"]
            //}

            //cartService.AddProductToCart(userId, cartId, request.itemId, request.quantity);

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

        [HttpDelete]
        public async Task<ActionResult> DeleteAllProducts()
        {
            //URL должен удалять все товары пользователя в таблице CartItems
            return BadRequest();
        }
    }
}
