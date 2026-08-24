using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;
using StackExchange.Redis;

namespace PrintShop.DataAccess.Repositories
{
    public class OrderRepository 
    {
        private readonly PrintShopDbContext _context;

        public OrderRepository()
        {
            
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
                        ProductId = x.ProductId
                    }).ToList();

                var order = new OrderEntity
                {
                    Id = orderId,
                    CreatedAt = DateTime.Now,
                    TotalAmount = cart.CalculateTotal(),
                    UserId = cart.UserId,
                    OrderItems = orderItems
                };

                //для каждой позиции в Cart отнять количество в Products
                await cart.Positions(x => await _context.Products.Where(w => w.Id == ))

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return (orderId, null);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return (null, ex.Message);
            }
        }
    }
}
