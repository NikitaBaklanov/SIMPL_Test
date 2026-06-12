using MediatR;
using OrderService.WebApi.Models;

namespace OrderService.WebApi.UseCases.Orders.GetOrder;

/// <summary>
/// Запрос на получение информации о заказе по идентификатору.
/// </summary>
public record GetOrderQuery(long Id) : IRequest<OrderResponse>;