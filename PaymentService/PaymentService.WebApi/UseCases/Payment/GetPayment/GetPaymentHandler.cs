using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentService.DataAccess.Postgres;
using PaymentService.WebApi.Models;

namespace PaymentService.WebApi.UseCases.Payments.GetPayment;

public class GetPaymentHandler : IRequestHandler<GetPaymentQuery, PaymentResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetPaymentHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaymentResponse> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
    {
        var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == request.PaymentId, cancellationToken);
        if (payment == null)
            throw new KeyNotFoundException($"Payment with id {request.PaymentId} not found");

        return _mapper.Map<PaymentResponse>(payment);
    }
}