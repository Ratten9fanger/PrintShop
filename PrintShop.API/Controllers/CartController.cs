using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces.Services;
using PrintShop.Domain.Models;

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
            Guid cartId = Guid.Empty;

            Guid? userId = null;

            var userIdClaim = HttpContext.User.FindFirst("userId");

            if (userIdClaim != null)
            {
                userId = Guid.Parse(HttpContext.User.FindFirst("userId").Value);

                cartId = Guid.Parse(HttpContext.User.FindFirst("cartId").Value);
            }
            else
            {
                cartId = Guid.NewGuid();

                Response.Cookies.Append("cartId", cartId.ToString()); //можно защитить куку

                Console.WriteLine($"userId is null");
            }

            //var result = await _cartService.AddPositionToCart(userId, cartId, request.productId, request.quantity);

            //return BadRequest();

            return Ok($"userId: {userId}, cartId: {cartId}");
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
