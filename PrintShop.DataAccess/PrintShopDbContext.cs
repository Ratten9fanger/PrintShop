using Microsoft.EntityFrameworkCore;
using PrintShop.DataAccess.Entities;

namespace PrintShop.DataAccess
{
    public class PrintShopDbContext : DbContext
    {
        public PrintShopDbContext(DbContextOptions<PrintShopDbContext> options) : base(options)
        {
            
        }

        public DbSet<UserEntity> Users { get; set; }
        
        public DbSet<CartEntity> Carts { get; set; }
        public DbSet<CartPositionEntity> CartPositions { get; set; }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }


    }
}
