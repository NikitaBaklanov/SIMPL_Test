namespace PaymentService.WebApi.Models;

// Запрос на создание платежа (приходит из OrderService)
public class CreatePaymentRequest
{
    public long OrderId { get; set; }
    public decimal Price { get; set; }
    public string EmailClient { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

// Ответ после создания
public class CreatePaymentResponse
{
    public long PaymentId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

// Ответ для GET /get/{paymentId}
public class PaymentResponse
{
    public long Id { get; set; }
    public decimal Price { get; set; }
    public bool Status { get; set; }
    public DateTime DateCreate { get; set; }
}