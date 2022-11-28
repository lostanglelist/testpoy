using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestKuy.Models;

namespace TestKuy.Models
{
    public class ImageDbContext : DbContext
    {
        public ImageDbContext()
        {
        }

        public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options)
        {

        }

        public DbSet<ImageModel> Images { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Shipping> Shippings { get; set; }

    }
}
