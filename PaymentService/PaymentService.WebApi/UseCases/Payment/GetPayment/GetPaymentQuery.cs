using MediatR;
using PaymentService.WebApi.Models;

namespace PaymentService.WebApi.UseCases.Payments.GetPayment;

public record GetPaymentQuery(long PaymentId) : IRequest<PaymentResponse>;