using Microsoft.EntityFrameworkCore;
using PayrollHrms.Models;

namespace PayrollHrms.Data
{
    public class PayrollHrmsDbContext : DbContext
    {
        public PayrollHrmsDbContext(DbContextOptions<PayrollHrmsDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
