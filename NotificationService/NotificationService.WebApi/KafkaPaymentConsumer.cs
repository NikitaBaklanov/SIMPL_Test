using Confluent.Kafka;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using NotificationService.WebApi.Hubs;
using NotificationService.WebApi.Models;

namespace NotificationService.WebApi.Services;

public class KafkaPaymentConsumer : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly ILogger<KafkaPaymentConsumer> _logger;
    private readonly string _topic = "payment-events";

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

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);
        return Task.Run(() => ConsumeLoop(stoppingToken), stoppingToken);
    }

    private async Task ConsumeLoop(CancellationToken stoppingToken)
    {
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
                        _logger.LogInformation("Received payment event for Order {OrderId}, email {Email}", paymentEvent.OrderId, paymentEvent.EmailClient);

                        // Отправляем уведомление через SignalR
                        await _hubContext.Clients.Group(paymentEvent.EmailClient)
                            .SendAsync("PaymentNotification", new
                            {
                                orderId = paymentEvent.OrderId,
                                price = paymentEvent.Price,
                                status = "Paid",
                                paidAt = paymentEvent.PaidAt
                            });

                        // Логирование
                        _logger.LogInformation("Sending payment notification to group {EmailClient} for order {OrderId}",
                            paymentEvent.EmailClient, paymentEvent.OrderId);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error consuming Kafka message");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }

    public override void Dispose()
    {
        _consumer.Close();
        _consumer.Dispose();
        base.Dispose();
    }
}