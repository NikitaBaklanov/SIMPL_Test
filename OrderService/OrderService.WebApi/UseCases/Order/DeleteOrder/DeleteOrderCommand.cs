using MediatR;

namespace OrderService.WebApi.UseCases.Orders.DeleteOrder;

/// <summary>
/// Команда на удаление заказа по идентификатору.
/// </summary>
public record DeleteOrderCommand(long Id) : IRequest;