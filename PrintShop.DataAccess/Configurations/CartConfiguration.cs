using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintShop.DataAccess.Entities;


namespace PrintShop.DataAccess.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<CartEntity>
    {
        public void Configure(EntityTypeBuilder<CartEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(u => u.User)
                .WithOne(c => c.Cart)
                .HasForeignKey<CartEntity>(c => c.UserId)
                .IsRequired(false);
        }
    }
}
 