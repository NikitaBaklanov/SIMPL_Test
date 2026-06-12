using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentService.DataAccess.Postgres.Configurations;
using PaymentService.DataAccess.Postgres.Models;

namespace PaymentService.DataAccess.Postgres
{
    /// <summary>
    /// Контекст базы данных для работы с платежами.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}

