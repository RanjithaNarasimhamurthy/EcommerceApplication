using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Tbl_User { get; set; } = default!;
        public DbSet<Vendor> Tbl_Vendor { get; set; } = default!;
        public DbSet<Category> Tbl_Category { get; set; } = default!;
        public DbSet<SubCategory> Tbl_SubCategory { get; set; } = default!;
        public DbSet<Address> Tbl_Address { get; set;} = default!;
        public DbSet<Order> Tbl_Order { get; set; } = default!;
        public DbSet<OrderItem> Tbl_OrderItem { get; set; } = default!;
        public DbSet<Product> Tbl_Product { get; set; } = default!;
        public DbSet<Feedback> Tbl_Feedback { get; set; } = default!;
        public DbSet<ProductDetails> Products { get; set; }
        public DbSet<ProductImageDetails> ProductImageDetails { get; set; }
        public DbSet<Wishlist> Tbl_Wishlist { get; set; }
        public DbSet<CartItem> Tbl_CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDetails>()
                .HasKey(p => p.strProductId);

            modelBuilder.Entity<ProductImageDetails>()
                .HasKey(pi => pi.intImageId);

            modelBuilder.Entity<ProductDetails>()
                .HasMany(p => p.ProductImageDetails)
                .WithOne()
                .HasForeignKey(pi => pi.strProductId);
            
            
        }
    }
}
