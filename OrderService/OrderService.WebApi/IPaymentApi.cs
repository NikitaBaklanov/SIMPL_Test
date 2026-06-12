using Refit;

namespace OrderService.WebApi.PaymentApi;

/// <summary>
/// Refit-клиент для взаимодействия с PaymentService.
/// </summary>
public interface IPaymentApi
{
    [Post("/api/payments/create")]
    Task<CreatePaymentResponse> CreatePaymentAsync([Body] CreatePaymentRequest request);
}

public class CreatePaymentRequest
{
    public long OrderId { get; set; }
    public decimal Price { get; set; }
    public string EmailClient { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class CreatePaymentResponse
{
    public long PaymentId { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}