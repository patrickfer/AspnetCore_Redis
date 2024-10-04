using AspnetCore_Redis.Models;
using Microsoft.EntityFrameworkCore;

namespace AspnetCore_Redis.Data
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; }
    }
}
