using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess.Postgres;
using OrderService.WebApi.UseCases.Orders.DeleteOrder;

namespace OrderService.WebApi.UseCases.Orders.DeleteOrder;

/// <summary>
/// Обработчик удаления заказа.
/// </summary>
public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly AppDbContext _context;
    private readonly ILogger<DeleteOrderHandler> _logger;

    public DeleteOrderHandler(AppDbContext context, ILogger<DeleteOrderHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);
        if (order == null)
        {
            _logger.LogWarning("Order {Id} not found for deletion", request.Id);
            throw new KeyNotFoundException($"Order with id {request.Id} not found");
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);
    }
}