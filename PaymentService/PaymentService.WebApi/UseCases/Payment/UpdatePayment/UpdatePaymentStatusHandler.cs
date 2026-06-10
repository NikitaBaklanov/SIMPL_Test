using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentService.DataAccess.Postgres;
using PaymentService.WebApi.Kafka;
using PaymentService.WebApi.Models;

namespace PaymentService.WebApi.UseCases.Payments.UpdatePaymentStatus;

public class UpdatePaymentStatusHandler : IRequestHandler<UpdatePaymentStatusCommand>
{
    private readonly AppDbContext _context;
    private readonly KafkaEventPublisher _kafkaPublisher;
    private readonly ILogger<UpdatePaymentStatusHandler> _logger;

    public UpdatePaymentStatusHandler(AppDbContext context, KafkaEventPublisher kafkaPublisher, ILogger<UpdatePaymentStatusHandler> logger)
    {
        _context = context;
        _kafkaPublisher = kafkaPublisher;
        _logger = logger;
    }

    public async Task Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == request.PaymentId, cancellationToken);
        if (payment == null)
            throw new KeyNotFoundException($"Payment with id {request.PaymentId} not found");

        if (payment.Status == true && request.IsPaid == true)
        {
            _logger.LogWarning("Payment {PaymentId} already paid", payment.Id);
            return;
        }

        payment.Status = request.IsPaid;
        payment.DateUpdate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Payment {PaymentId} status updated to {Status}", payment.Id, request.IsPaid);

        if (request.IsPaid)
        {
            var paymentEvent = new PaymentCompletedEvent
            {
                OrderId = payment.OrderId,
                EmailClient = payment.EmailClient,
                PhoneNumber = payment.PhoneNumber,
                Price = payment.Price,
                PaidAt = DateTime.UtcNow
            };
            await _kafkaPublisher.PublishPaymentCompletedAsync(paymentEvent);
            _logger.LogInformation("Published payment completed event for Order {OrderId}", payment.OrderId);
        }
    }
}