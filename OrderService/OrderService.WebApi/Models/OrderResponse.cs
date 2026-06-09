namespace OrderService.WebApi.Models;

public class OrderResponse
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public int Amount { get; set; }
    public string EmailClient { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}