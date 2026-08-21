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
            builder.HasKey(x => x.Id);

            builder.Property(b => b.Title).HasMaxLength().IsRequired();

            builder.HasOne(u => u.User)

                //.WithOne(p => p.Category)
                //.HasForeignKey(p => p.CategoryId)
                //.OnDelete(DeleteBehavior.Cascade);
        }
    }
}
 