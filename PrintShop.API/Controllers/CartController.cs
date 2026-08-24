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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var userIdClaim = HttpContext.User.FindFirst("userId");

            var userId = Guid.Parse(userIdClaim!.Value);

            var cart = await _cartService.GetCart(userId);

            //var total = cart.CalculateTotal();

            return Ok(cart);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Guid>> AddProduct([FromBody] CartPositionRequest request)
        {
            var userIdClaim = HttpContext.User.FindFirst("userId");

            var userId = Guid.Parse(userIdClaim!.Value);

            var result = await _cartService.AddPositionToCart(userId, request.productId, request.quantity);

            if (result.Error != null)
                return BadRequest(result.Error);

            return Ok(result.PositionId);
        }

        [HttpPost()]
        [Authorize]
        public async Task<ActionResult<Guid>> Buy()
        {
            var userIdClaim = HttpContext.User.FindFirst("userId");

            var userId = Guid.Parse(userIdClaim!.Value);

            var result = await _cartService.CreateOrder(id);

            return Ok(orderId)
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
