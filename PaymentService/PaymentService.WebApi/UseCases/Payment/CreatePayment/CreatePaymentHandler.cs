using MediatR;
using PaymentService.DataAccess.Postgres;
using PaymentService.DataAccess.Postgres.Models;
using PaymentService.WebApi.Models;

namespace PaymentService.WebApi.UseCases.Payments.CreatePayment;


/// <summary>
/// Обработчик создания платежа. Сохраняет запись в БД со статусом false (не оплачен).
/// </summary>
public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentResponse>
{
    private readonly AppDbContext _context;
    private readonly ILogger<CreatePaymentHandler> _logger;

    public CreatePaymentHandler(AppDbContext context, ILogger<CreatePaymentHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<CreatePaymentResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new Payment
        {
            OrderId = request.OrderId,
            Price = request.Price,
            Status = false,
            DateCreate = DateTime.UtcNow,
            EmailClient = request.EmailClient,
            PhoneNumber = request.PhoneNumber
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Payment created with Id {PaymentId} for Order {OrderId}", payment.Id, payment.OrderId);

        return new CreatePaymentResponse
        {
            PaymentId = payment.Id,
            Success = true,
            Message = "Payment reserved"
        };
    }
}