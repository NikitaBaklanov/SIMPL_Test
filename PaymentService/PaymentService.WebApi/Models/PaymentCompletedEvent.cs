namespace PaymentService.WebApi.Models;

/// <summary>
/// Событие об успешной оплате, отправляемое в Kafka.
/// </summary>
public class PaymentCompletedEvent
{
    public long OrderId { get; set; }
    public string EmailClient { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime PaidAt { get; set; }
}