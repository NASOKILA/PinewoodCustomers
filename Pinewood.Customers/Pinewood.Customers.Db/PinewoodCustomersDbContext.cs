using Microsoft.EntityFrameworkCore;
using Pinewood.Customers.Models.DbModels;
namespace Pinewood.Customers.Db
{
    public class PinewoodCustomersDbContext : DbContext
    {
        public PinewoodCustomersDbContext(DbContextOptions<PinewoodCustomersDbContext> options)
            : base(options)
        {}

        public DbSet<Customer> Customers { get; set; }
    }
}
