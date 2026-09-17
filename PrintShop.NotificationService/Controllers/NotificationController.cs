using Microsoft.AspNetCore.Mvc;
using PrintShop.NotificationService.Contracts;

namespace PrintShop.NotificationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        [HttpPost("order-notification")]
        public async Task<ActionResult<string>> GetData([FromBody] OrderDto dto)
        {
            _logger.LogInformation("Получен HTTP-запрос: Заказ {Id} на сумму {TotalAmount}",
                dto.Id, dto.TotalAmount);

            await Task.Delay(1000);

            _logger.LogInformation("Email отправлен для заказа {OrderId}", dto.Id);

            return Ok();
        }
    }
}
