using Microsoft.EntityFrameworkCore;

namespace Order.Api.Models.Contexts
{
    public class OrderApiDbContext : DbContext
    {
        public OrderApiDbContext(DbContextOptions<OrderApiDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
