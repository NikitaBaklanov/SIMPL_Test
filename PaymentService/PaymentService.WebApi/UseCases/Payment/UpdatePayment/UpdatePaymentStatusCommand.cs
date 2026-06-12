using MediatR;

namespace PaymentService.WebApi.UseCases.Payments.UpdatePaymentStatus;

/// <summary>
/// Команда на обновление статуса платежа (оплачен/не оплачен).
/// </summary>
public record UpdatePaymentStatusCommand(long PaymentId, bool IsPaid) : IRequest;