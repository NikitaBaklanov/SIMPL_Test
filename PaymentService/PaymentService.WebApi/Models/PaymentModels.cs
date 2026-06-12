namespace PaymentService.WebApi.Models;

/// <summary>
/// Запрос на создание платежа (приходит из OrderService).
/// </summary>
public class CreatePaymentRequest
{
    public long OrderId { get; set; }
    public decimal Price { get; set; }
    public string EmailClient { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

/// <summary>
/// Ответ на создание платежа.
/// </summary>
public class CreatePaymentResponse
{
    public long PaymentId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

/// <summary>
/// Ответ при получении информации о платеже (GET).
/// </summary>
public class PaymentResponse
{
    public long Id { get; set; }
    public decimal Price { get; set; }
    public bool Status { get; set; }
    public DateTime DateCreate { get; set; }
}