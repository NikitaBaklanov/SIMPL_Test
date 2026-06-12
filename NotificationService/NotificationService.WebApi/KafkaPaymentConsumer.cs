using Confluent.Kafka;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using NotificationService.WebApi.Hubs;
using NotificationService.WebApi.Models;

namespace NotificationService.WebApi.Services;

/// <summary>
/// Фоновый сервис для чтения событий из Kafka (топик payment-events)
/// и рассылки уведомлений клиентам через SignalR.
/// </summary>
public class KafkaPaymentConsumer : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<KafkaPaymentConsumer> _logger;
    private readonly string _topic = "payment-events";
    private bool _disposed = false;

    public KafkaPaymentConsumer(IConfiguration configuration, IHubContext<NotificationHub> hubContext, ILogger<KafkaPaymentConsumer> logger)
    {
        var bootstrapServers = configuration["KafkaSettings:BootstrapServers"] ?? "kafka:29092";
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = "notification-service-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = true
        };
        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        _hubContext = hubContext;
        _logger = logger;
    }

    /// <summary>
    /// Запуск consumer'а при старте сервиса.
    /// </summary>
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(_topic);
        _logger.LogInformation("Subscribed to topic {Topic}", _topic);
        return base.StartAsync(cancellationToken);
    }

    /// <summary>
    /// Основной цикл обработки сообщений.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Освобождаем поток, чтобы не блокировать запуск
        await Task.Yield();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                if (consumeResult?.Message?.Value != null)
                {
                    var paymentEvent = JsonSerializer.Deserialize<PaymentCompletedEvent>(consumeResult.Message.Value);
                    if (paymentEvent != null)
                    {
                        _logger.LogInformation("Received payment event for Order {OrderId}, email {Email}",
                            paymentEvent.OrderId, paymentEvent.EmailClient);

                        // Отправляем уведомление через SignalR всем клиентам, подписанным на группу email
                        await _hubContext.Clients.Group(paymentEvent.EmailClient)
                            .SendAsync("PaymentNotification", new
                            {
                                orderId = paymentEvent.OrderId,
                                price = paymentEvent.Price,
                                status = "Paid",
                                paidAt = paymentEvent.PaidAt
                            });

                        _logger.LogInformation("Payment notification sent to group {EmailClient} for order {OrderId}",
                            paymentEvent.EmailClient, paymentEvent.OrderId);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to deserialize Kafka message: {RawValue}", consumeResult.Message.Value);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Нормальная остановка
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming Kafka message");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    /// <summary>
    /// Остановка consumer'а при завершении сервиса.
    /// </summary>
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.Close();
        _logger.LogInformation("Kafka consumer closed");
        return base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        if (!_disposed)
        {
            _consumer.Dispose();
            _disposed = true;
        }
        base.Dispose();
    }
}