using Microsoft.EntityFrameworkCore;
using PrintShop.DataAccess.Configurations;
using PrintShop.DataAccess.Entities;

namespace PrintShop.DataAccess
{
    public class PrintShopDbContext : DbContext
    {
        public PrintShopDbContext(DbContextOptions<PrintShopDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CategoryConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderItemConfiguration).Assembly);
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }


    }
}
