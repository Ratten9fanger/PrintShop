using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;

namespace PrintShop.DataAccess.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            // Настройка первичного ключа
            builder.HasKey(o => o.Id);

            // НАСТРОЙКА СВЯЗИ С ЭЛЕМЕНТАМИ ЗАКАЗА:
            builder.HasMany(o => o.OrderItems)      // У заказа МНОГО элементов
                   .WithOne(oi => oi.Order)          // У каждого элемента ОДИН заказ
                   .HasForeignKey(oi => oi.OrderId)  // Внешний ключ лежит в OrderItemEntity
                   .OnDelete(DeleteBehavior.Cascade); // Если удаляется заказ, удаляются и его элементы
        }
    }
}
 