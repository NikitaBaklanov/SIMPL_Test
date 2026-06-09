using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.WebApi.Models;
using OrderService.WebApi.UseCases.Orders.CreateOrder;
using OrderService.WebApi.UseCases.Orders.GetOrder;
using OrderService.WebApi.UseCases.Orders.DeleteOrder;

namespace OrderService.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<ActionResult<OrderResponse>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var command = new CreateOrderCommand(request);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponse>> GetOrder(long id)
    {
        var query = new GetOrderQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(long id)
    {
        var command = new DeleteOrderCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}