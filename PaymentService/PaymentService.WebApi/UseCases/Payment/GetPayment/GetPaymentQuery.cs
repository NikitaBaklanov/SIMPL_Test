using MediatR;
using PaymentService.WebApi.Models;

namespace PaymentService.WebApi.UseCases.Payments.GetPayment;

/// <summary>
/// Запрос на получение платежа по идентификатору.
/// </summary>
public record GetPaymentQuery(long PaymentId) : IRequest<PaymentResponse>;