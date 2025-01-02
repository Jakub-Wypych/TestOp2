using Microsoft.EntityFrameworkCore;
using ProductApp.Models;
using System.Collections.Generic;

namespace ProductApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}
