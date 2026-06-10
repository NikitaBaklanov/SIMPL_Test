using NotificationService.WebApi.Hubs;
using NotificationService.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddHostedService<KafkaPaymentConsumer>();

// CORS для тестирования из браузера
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.MapHub<NotificationHub>("/notificationHub");

// Эндпоинт для проверки работоспособности
app.MapGet("/health", () => "Notification Service is running");

app.Run();