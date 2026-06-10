using MediatR;
using PaymentService.WebApi.Models;

namespace PaymentService.WebApi.UseCases.Payments.CreatePayment;

public record CreatePaymentCommand(
    long OrderId,
    decimal Price,
    string EmailClient,
    string PhoneNumber
) : IRequest<CreatePaymentResponse>;