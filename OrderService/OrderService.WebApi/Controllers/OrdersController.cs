using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.WebApi.Models;
using OrderService.WebApi.UseCases.Orders.CreateOrder;
using OrderService.WebApi.UseCases.Orders.GetOrder;
using OrderService.WebApi.UseCases.Orders.DeleteOrder;

namespace OrderService.WebApi.Controllers;


/// <summary>
/// Контроллер для работы с заказами.
/// </summary>
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Создать новый заказ.
    /// </summary>
    [HttpPost("create")]
    public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var command = new CreateOrderCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }


    /// <summary>
    /// Получить заказ по идентификатору.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetOrder(long id)
    {
        var query = new GetOrderQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    /// <summary>
    /// Удалить заказ по идентификатору.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(long id)
    {
        var command = new DeleteOrderCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}