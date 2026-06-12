namespace OrderService.WebApi.Models;

/// <summary>
/// Модель запроса на создание заказа, приходящая от клиента.
/// </summary>
public class CreateOrderRequest
{
    public long ProductId { get; set; }
    public int Amount { get; set; }
    public string EmailClient { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
}