using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Postgres.Models;
using OrderService.DataAccess.Postgres.Configurations;

namespace OrderService.DataAccess.Postgres;

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