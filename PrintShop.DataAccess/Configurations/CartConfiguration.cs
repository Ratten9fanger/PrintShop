using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintShop.Domain.Models;

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
 