using Microsoft.EntityFrameworkCore;
using ProductApp.Models;
using System.Collections.Generic;

namespace ProductApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure validation for the Product entity
            modelBuilder.Entity<Product>(entity =>
            {
                // Name
                entity.Property(p => p.Name)
                    .IsRequired() // Name is required
                    .HasMaxLength(100); // Max length of 100 characters

                // Date
                entity.Property(p => p.Date)
                    .IsRequired(); // Date is required

                // Price
                entity.Property(p => p.Price)
                    .IsRequired() // Price is required
                    .HasColumnType("decimal(18,2)") // Define the type to be decimal with precision
                    .HasDefaultValue(0); // Optional: you can add a default value, if desired

                // Category
                entity.Property(p => p.Category)
                    .IsRequired() // Category is required
                    .HasMaxLength(50); // Max length of 50 characters

                // Quantity
                entity.Property(p => p.Quantity)
                    .IsRequired() // Quantity is required
                    .HasDefaultValue(1); // Optional: you can add a default value

                // Other custom configurations can go here
            });
        }
    }
}
