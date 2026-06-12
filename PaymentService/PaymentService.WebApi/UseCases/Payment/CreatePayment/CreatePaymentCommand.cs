using MediatR;
using PaymentService.WebApi.Models;

namespace PaymentService.WebApi.UseCases.Payments.CreatePayment;

/// <summary>
/// Команда на создание нового платежа (резервирование средств).
/// </summary>
public record CreatePaymentCommand(
    long OrderId,
    decimal Price,
    string EmailClient,
    string PhoneNumber
) : IRequest<CreatePaymentResponse>;