using MediatR;
using OrderService.WebApi.Models;

namespace OrderService.WebApi.UseCases.Orders.CreateOrder;

/// <summary>
/// Команда на создание нового заказа.
/// </summary>
public record CreateOrderCommand(CreateOrderRequest Request) : IRequest<OrderResponse>;