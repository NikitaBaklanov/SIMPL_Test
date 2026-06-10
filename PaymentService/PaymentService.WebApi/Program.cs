using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.DataAccess.Postgres;
using PaymentService.WebApi.GlobalExceptionMiddleware;
using PaymentService.WebApi.Kafka;
using PaymentService.WebApi.Mappings;
using PaymentService.WebApi.UseCases.Payments.CreatePayment;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreatePaymentCommandValidator>();

// AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

// Kafka Publisher (singleton)
builder.Services.AddSingleton<KafkaEventPublisher>();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();