using ECommerceAspNetCore.Models.Entities;
using ECommerceAspNetCore.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAspNetCore.Data
{
    public class MyContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {

        }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<CartProducts> CartProducts { get; set; }

        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>()
                .Property(x => x.Price)
                .HasPrecision(8, 2);

            builder.Entity<CartProducts>()
                .Property(x => x.Price)
                .HasPrecision(8, 2);

            builder.Entity<CartProducts>()
                .HasKey(x => new { x.CartId, x.ProductId});

            builder.Entity<ApplicationUser>()
                .HasOne(x => x.Cart)
                .WithOne(q => q.ApplicationUser)
                .HasForeignKey<Cart>(q => q.ApplicationUserId);
        }
    }
}
