using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Postgres;
using OrderService.WebApi.Models;

namespace OrderService.WebApi.UseCases.Orders.GetOrder;

public class GetOrderHandler : IRequestHandler<GetOrderQuery, OrderResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OrderResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        if (order == null)
            throw new Exception($"Order with id {request.Id} not found");

        return _mapper.Map<OrderResponse>(order);
    }
}