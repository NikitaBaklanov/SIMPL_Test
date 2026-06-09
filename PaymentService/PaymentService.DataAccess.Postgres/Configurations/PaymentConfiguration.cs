using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.DataAccess.Postgres.Models;

namespace PaymentService.DataAccess.Postgres.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        builder.Property(p => p.OrderId).IsRequired();
        builder.Property(p => p.Price).HasPrecision(18, 2).IsRequired();
        builder.Property(p => p.Status).IsRequired();
        builder.Property(p => p.DateCreate).IsRequired();
        builder.Property(p => p.EmailClient).IsRequired().HasMaxLength(256);
        builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(20);
    }
}