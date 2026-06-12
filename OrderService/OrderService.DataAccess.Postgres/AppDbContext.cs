using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.DataAccess.Postgres.Configurations;
using OrderService.DataAccess.Postgres.Models;

namespace OrderService.DataAccess.Postgres
{
    /// <summary>
    /// Контекст базы данных для работы с заказами.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}

