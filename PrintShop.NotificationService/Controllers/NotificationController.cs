using Microsoft.AspNetCore.Mvc;
using PrintShop.NotificationService.Contracts;

namespace PrintShop.NotificationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : Controller
    {
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(ILogger<NotificationController> logger)
        {
            _logger = logger;
        }

        [HttpPost("order-created")]
        public async Task<IActionResult> SendOrderNotification([FromBody] OrderDto dto)
        {
            _logger.LogInformation(
                "Получен HTTP-запрос: Заказ {Id} для {CreatedAt} на сумму {TotalAmount}₽",
                dto.Id, dto.CreatedAt, dto.TotalAmount);

            // Имитация отправки email
            await Task.Delay(500); // 500ms задержка

            _logger.LogInformation("✅ Email отправлен для заказа {OrderId}", dto.Id);

            return Ok(new { Message = "Уведомление отправлено" });
        }
    }
}
