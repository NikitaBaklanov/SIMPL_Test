using MediatR;

namespace OrderService.WebApi.UseCases.Orders.DeleteOrder;

public record DeleteOrderCommand(long Id) : IRequest;