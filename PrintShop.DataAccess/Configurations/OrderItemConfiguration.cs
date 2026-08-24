using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintShop.DataAccess.Entities;
using PrintShop.Domain.Models;

namespace PrintShop.DataAccess.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder.HasKey(x => x.Id);

            // НАСТРОЙКА СВЯЗИ С ПРОДУКТОМ:
            builder.HasOne(oi => oi.Product)       // У элемента заказа есть ОДИН продукт
                   .WithMany()                     // А у продукта МНОГО элементов заказа (но свойства-коллекции в ProductEntity нет, оставляем пустым)
                   .HasForeignKey(oi => oi.ProductId) // Внешний ключ находится здесь
                   .OnDelete(DeleteBehavior.Restrict); // Запрещаем удалять продукт, если он есть в заказах
        }
    }
}
 