using MediatR;
using OrderService.WebApi.Models;

namespace OrderService.WebApi.UseCases.Orders.GetOrder;

public record GetOrderQuery(long Id) : IRequest<OrderResponse>;