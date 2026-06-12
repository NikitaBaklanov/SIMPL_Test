using Confluent.Kafka;
using System.Text.Json;
using PaymentService.WebApi.Models;

namespace PaymentService.WebApi.Kafka;

/// <summary>
/// Публикатор событий в Kafka. Отправляет сообщение об успешной оплате в топик payment-events.
/// </summary>
public class KafkaEventPublisher : IDisposable
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic = "payment-events";

    public KafkaEventPublisher(IConfiguration configuration)
    {
        var bootstrapServers = configuration["KafkaSettings:BootstrapServers"] ?? "kafka:29092";
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
            EnableIdempotence = true,
            Acks = Acks.All
        };
        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
    }

    public async Task PublishPaymentCompletedAsync(PaymentCompletedEvent paymentEvent)
    {
        var json = JsonSerializer.Serialize(paymentEvent);
        await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = json });
    }

    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}