using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using POS.DataModels;

namespace POS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<SellHistory> SellHistories { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure the default Identity configuration is applied

            // Configure one-to-one relationship between Product and ProductStock
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Stock)
                .WithOne(ps => ps.Product)
                .HasForeignKey<ProductStock>(ps => ps.ProductId);

            // Configure one-to-many relationship between Product and SellHistory
            modelBuilder.Entity<Product>()
                .HasMany(p => p.SellHistory)
                .WithOne(sh => sh.Product)
                .HasForeignKey(sh => sh.ProductId);
        }
    }
}
