using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace PrintShop.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PrintShopDbContext _context;
        private readonly string _connectionString;
        private readonly ILogger<OrderRepository> _logger;


        public OrderRepository(PrintShopDbContext context, IConfiguration configuration, ILogger<OrderRepository> logger, IHttpClientFactory httpClientFactory)
        {
            _connectionString = configuration.GetConnectionString("Default")!;
            _context = context;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<(OrderDto? OrderDto, string? Error)> CreateOrder(Cart cart)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var orderId = Guid.NewGuid();

                var orderItems = cart.Positions
                    .Select(x => new OrderItemEntity
                    {
                        Id = Guid.NewGuid(),
                        Quantity = x.Quantity,
                        PriceAtMoment = x.PriceAtMoment,
                        OrderId = orderId,
                        ProductId = x.ProductId,
                        ProductName = _context.Products.Find(x.ProductId)!.Title
                    }).ToList();

                var order = new OrderEntity
                {
                    Id = orderId,
                    CreatedAt = DateTime.UtcNow,
                    TotalAmount = cart.CalculateTotal(),
                    UserId = cart.UserId,
                    OrderItems = orderItems
                };

                // Для каждой позиции делаем атомарное обновление: уменьшить сток, но только если его достаточно
                foreach (var position in cart.Positions)
                {
                    var affectedRows = await _context.Products
                        .Where(p => p.Id == position.ProductId && p.StockQuantity >= position.Quantity)
                        .ExecuteUpdateAsync(s => s.SetProperty(p => p.StockQuantity, p => p.StockQuantity - position.Quantity));

                    if (affectedRows == 0)
                    {
                        // Если 0 строк обновлено, значит товара не хватило (кто-то перехватил его между добавлением в корзину и чекаутом)
                        _logger.LogInformation("Товар {position} закончился или его количество изменилось", position);
                        return (null, $"Товар с ID {position.ProductId} закончился или его количество изменилось.");
                    }
                }

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                _logger.LogInformation("Заказ {order} успешно обработан", order);

                var orderDto = new OrderDto(
                        order.Id,
                        order.CreatedAt,
                        order.TotalAmount,
                        order.OrderItems.Select(oi => new OrderItemDto(oi.ProductName, oi.Quantity, oi.PriceAtMoment)).ToList());

                try
                {
                    var httpClient = _httpClientFactory.CreateClient("NotificationService");

                    var response = await httpClient.PostAsJsonAsync("http://localhost:7001/notification/order-notification", orderDto);

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Не удалось отправить уведомление. Status: {response.StatusCode}", response.StatusCode);
                    }
                    else
                    {
                        _logger.LogInformation("Уведомление отправлено для заказа {orderId}", orderId);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Возникла ошибка при отправке уведомления для заказа {orderId} - {ex}", orderId, ex);
                }

                return (orderDto, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogInformation("Ошибка при выполнении заказа у {cart.UserId} - {ex.Message},",cart.UserId, ex.Message);
                return (null, "Can't make an order now, try again later");
            }
        }

        public async Task<List<OrderDto>?> GetByUserId(Guid userId)
        {
            var orderEntities = await _context.Orders
                .Where(x => x.UserId == userId)
                .Include(x => x.OrderItems)
                .ToListAsync();

            if (orderEntities == null) return null;

            var orderDtos = orderEntities
                .Select(x => new OrderDto(
                    x.Id,
                    x.CreatedAt,
                    x.TotalAmount,
                    x.OrderItems.Select(oi => new OrderItemDto(oi.ProductName, oi.Quantity, oi.PriceAtMoment)).ToList())
                 ).ToList();

            return orderDtos;   
        }
    }
}
