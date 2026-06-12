namespace NotificationService.WebApi.Models;

/// <summary>
/// Модель события об успешной оплате, получаемая из Kafka.
/// Соответствует сообщению, отправляемому PaymentService.
/// </summary>
public class PaymentCompletedEvent
{
    public long OrderId { get; set; }
    public string EmailClient { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty; // не используется в SignalR, но может пригодиться в будующем
    public decimal Price { get; set; }
    public DateTime PaidAt { get; set; }
}