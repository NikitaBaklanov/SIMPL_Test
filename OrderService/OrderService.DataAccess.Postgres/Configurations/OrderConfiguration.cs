using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.DataAccess.Postgres.Models;

namespace OrderService.DataAccess.Postgres.Configurations;

/// <summary>
/// Конфигурация сущности Order для Entity Framework Core (Fluent API).
/// </summary>
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedOnAdd();
        builder.Property(o => o.ProductId).IsRequired();
        builder.Property(o => o.Amount).IsRequired();
        builder.Property(o => o.EmailClient).IsRequired().HasMaxLength(256);
        builder.Property(o => o.Price).HasPrecision(18, 2).IsRequired();
        builder.Property(o => o.PhoneNumber).IsRequired().HasMaxLength(20);
        builder.Property(o => o.CreatedAt).IsRequired();
        builder.Property(o => o.Status).IsRequired().HasMaxLength(50);
    }
}