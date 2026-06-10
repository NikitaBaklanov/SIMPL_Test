using NotificationService.WebApi.Hubs;
using NotificationService.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Контроллеры (опционально, для health check)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR
builder.Services.AddSignalR();

// Фоновый сервис для потребления Kafka
builder.Services.AddHostedService<KafkaPaymentConsumer>();

// Настройка CORS для SignalR (разрешаем любые источники с credentials)
builder.Services.AddCors(options =>
{
    options.AddPolicy("SignalRCors", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();              // необходимо для SignalR
    });
});

var app = builder.Build();

// Настройка конвейера HTTP-запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Перенаправление на HTTPS
// app.UseHttpsRedirection();

// Используем CORS
app.UseCors("SignalRCors");

// Эндпоинт для проверки работоспособности
app.MapGet("/health", () => Results.Ok(new { status = "Notification Service is running" }));

// SignalR Hub
app.MapHub<NotificationHub>("/notificationHub");

// Опционально: контроллеры (если есть)
app.MapControllers();

app.Run();