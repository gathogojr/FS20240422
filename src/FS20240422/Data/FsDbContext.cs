using FS20240422.Models;
using Microsoft.EntityFrameworkCore;

namespace FS20240422.Data
{
    public class FsDbContext : DbContext
    {
        public FsDbContext(DbContextOptions<FsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
