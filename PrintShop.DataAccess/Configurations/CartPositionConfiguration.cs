using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintShop.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintShop.DataAccess.Configurations
{
    public class CartPositionConfiguration : IEntityTypeConfiguration<CartPositionEntity>
    {
        public void Configure(EntityTypeBuilder<CartPositionEntity> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
 