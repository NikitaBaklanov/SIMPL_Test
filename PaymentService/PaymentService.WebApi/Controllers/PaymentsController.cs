using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.WebApi.Models;
using PaymentService.WebApi.UseCases.Payments.CreatePayment;
using PaymentService.WebApi.UseCases.Payments.GetPayment;
using PaymentService.WebApi.UseCases.Payments.UpdatePaymentStatus;

namespace PaymentService.WebApi.Controllers;


/// <summary>
/// Контроллер для управления платежами.
/// </summary>
[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Создать новый платеж.
    /// </summary>
    [HttpPost("create")]
    public async Task<ActionResult<CreatePaymentResponse>> CreatePayment([FromBody] CreatePaymentRequest request)
    {
        var command = new CreatePaymentCommand(request.OrderId, request.Price, request.EmailClient, request.PhoneNumber);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Обновляем статус платежа по идентификатору.
    /// </summary>
    [HttpPut("updateStatus/{paymentId}/{statusId}")]
    public async Task<IActionResult> UpdatePaymentStatus(long paymentId, int statusId)
    {
        var isPaid = statusId == 1;
        await _mediator.Send(new UpdatePaymentStatusCommand(paymentId, isPaid));
        return Ok(new { message = "Status updated" });
    }

    /// <summary>
    /// Получить информацию о платеже по идентификатору.
    /// </summary>
    [HttpGet("get/{paymentId}")]
    public async Task<ActionResult<PaymentResponse>> GetPayment(long paymentId)
    {
        var result = await _mediator.Send(new GetPaymentQuery(paymentId));
        return Ok(result);
    }
}