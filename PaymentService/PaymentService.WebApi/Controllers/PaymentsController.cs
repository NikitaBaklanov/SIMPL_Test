using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.WebApi.Models;
using PaymentService.WebApi.UseCases.Payments.CreateOrder;
using PaymentService.WebApi.UseCases.Payments.GetOrder;
using PaymentService.WebApi.UseCases.Payments.DeleteOrder;

namespace PaymentService.WebApi.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public PaymentsController(IMediator mediator) => _mediator = mediator;

    [HttpPost("create")]
    public async Task<ActionResult<CreatePaymentResponse>> Create([FromBody] CreatePaymentCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPut("updateStatus/{paymentId}/{statusId}")]
    public async Task<IActionResult> UpdateStatus(long paymentId, int statusId)
    {
        await _mediator.Send(new UpdatePaymentStatusCommand(paymentId, statusId == 1));
        return Ok();
    }

    [HttpGet("get/{paymentId}")]
    public async Task<ActionResult<PaymentResponse>> Get(long paymentId)
        => Ok(await _mediator.Send(new GetPaymentQuery(paymentId)));
}