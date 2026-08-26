using Microsoft.AspNetCore.Mvc;
using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces.Repositories;

namespace PrintShop.API.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public ActionResult<List<OrderDto>> Get()
        {
            var userIdClaim = HttpContext.User.FindFirst("userId");
            var userId = Guid.Parse(userIdClaim!.Value);

            var dtos = _orderRepository.GetByUserId(userId);

            return Ok(dtos);
        }
    }
}
