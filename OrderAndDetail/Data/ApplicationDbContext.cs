using Microsoft.EntityFrameworkCore;

namespace OrderAndDetail.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
        }
        public DbSet<OrderAndDetail.Models.Entities.Orders> Orders { get; set; }
        public DbSet<OrderAndDetail.Models.Entities.OrderDetail> orderDetails { get; set; }
    }
}
