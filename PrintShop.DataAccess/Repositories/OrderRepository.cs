using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PrintShop.Application.Dtos;
using PrintShop.Application.Interfaces.Repositories;
using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;
using System.Linq;

namespace PrintShop.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly PrintShopDbContext _context;

        private readonly string _connectionString;


        public OrderRepository(PrintShopDbContext context, IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default")!;
            _context = context;
        }

        public async Task<(Guid? OrderId, string? Error)> CreateOrder(Cart cart)
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
                        return (null, $"Товар с ID {position.ProductId} закончился или его количество изменилось.");
                    }
                }

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return (orderId, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"ошибка при выполнении заказа - {ex.Message}");
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
