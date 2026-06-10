using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.DataAccess.Postgres.Configurations;
using OrderService.DataAccess.Postgres.Models;

namespace OrderService.DataAccess.Postgres
{
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

    /*
    public static class DatabaseInitializer
    {
        // Создаёт базу данных и все таблицы, если они ещё не существуют.
        public static void Initialize(AppDbContext context, ILogger logger = null)
        {
            bool created = context.Database.EnsureCreated();
            if (created)
                logger?.LogInformation("База данных и таблицы успешно созданы (EnsureCreated).");
            else
                logger?.LogInformation("База данных уже существует. Создание не требуется.");
        }
    }
    */
}

