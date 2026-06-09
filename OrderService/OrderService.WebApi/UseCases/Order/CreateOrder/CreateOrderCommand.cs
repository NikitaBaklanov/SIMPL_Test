using MediatR;
using OrderService.WebApi.Models;

namespace OrderService.WebApi.UseCases.Orders.CreateOrder;

public record CreateOrderCommand(CreateOrderRequest Request) : IRequest<OrderResponse>;