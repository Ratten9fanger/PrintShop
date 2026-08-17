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
            var cartIdClaim = HttpContext.User.FindFirst("cartId");

            if (cartIdClaim == null) Console.WriteLine("юзер не авторизован, берем айди из куки...");

            var cartId = cartIdClaim?.Value ?? Request.Cookies["cartId"];

            Console.WriteLine("айди из куки ", cartId);

            Console.BackgroundColor = ConsoleColor.Yellow;

            var cart = await _cartService.GetCart(Guid.Parse(cartId!));

            return Ok(cart);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Guid>> AddProduct([FromBody] CartPositionRequest request)
        {
            Guid cartId = Guid.Empty;

            Guid? userId = null;

            var userIdClaim = HttpContext.User.FindFirst("userId");

            if (userIdClaim != null)
            {
                userId = Guid.Parse(userIdClaim!.Value);

                cartId = Guid.Parse(HttpContext.User.FindFirst("cartId")!.Value);
            }
            else
            {
                if (Request.Cookies["cartId"] == null)
                {
                    Response.Cookies.Append(
                        "cartId",
                        Guid.NewGuid().ToString(),
                        new CookieOptions
                        {
                            Expires = DateTimeOffset.Now.AddHours(24),
                            HttpOnly = true,
                            SameSite = SameSiteMode.Strict
                        }
                    );
                }

                cartId = Guid.Parse(Request.Cookies["cartId"]!);

                Console.WriteLine("юзер не авторизован");
            }

            var result = await _cartService.AddPositionToCart(userId, cartId, request.productId, request.quantity);

            if (result.Error != null)
                return BadRequest(result.Error);

            return Ok(result.PositionId);
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
