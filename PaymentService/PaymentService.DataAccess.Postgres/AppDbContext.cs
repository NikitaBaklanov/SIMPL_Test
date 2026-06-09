using Microsoft.EntityFrameworkCore;
using PaymentService.DataAccess.Postgres.Models;
using PaymentService.DataAccess.Postgres.Configurations;

namespace PaymentService.DataAccess.Postgres;

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