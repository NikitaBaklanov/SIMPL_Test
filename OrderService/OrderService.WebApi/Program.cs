using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService.DataAccess.Postgres;
using OrderService.WebApi.PaymentApi;
using OrderService.WebApi.GlobalExceptionMiddleware;
using OrderService.WebApi.Pipeline;
using OrderService.WebApi.Mappings;
using OrderService.WebApi.UseCases.Orders.CreateOrder;
using Refit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

//AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

//FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();

//Валидация
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

//Refit client for PaymentService
builder.Services.AddRefitClient<IPaymentApi>()
    .ConfigureHttpClient(c =>
    {
        var baseUrl = builder.Configuration["IntegrationSettings:PaymentServiceUrl"];
        c.BaseAddress = new Uri(baseUrl ?? "http://payment-service:8080/");
    });

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();

//Auto apply migrations
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        dbContext.Database.Migrate();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapControllers();
app.Run();