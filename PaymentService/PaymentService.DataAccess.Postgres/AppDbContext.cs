using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaymentService.DataAccess.Postgres.Configurations;
using PaymentService.DataAccess.Postgres.Models;

namespace PaymentService.DataAccess.Postgres
{
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

