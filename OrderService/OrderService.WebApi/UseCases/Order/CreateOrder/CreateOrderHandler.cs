using AutoMapper;
using MediatR;
using OrderService.DataAccess.Postgres;
using OrderService.DataAccess.Postgres.Models;
using OrderService.WebApi.PaymentApi;
using OrderService.WebApi.Models;

namespace OrderService.WebApi.UseCases.Orders.CreateOrder;

/// <summary>
/// Обработчик команды создания заказа.
/// Сохраняет заказ в БД, вызывает PaymentService для резервирования оплаты,
/// обновляет статус заказа в зависимости от результата.
/// </summary>
public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPaymentApi _paymentApi;
    private readonly ILogger<CreateOrderHandler> _logger;

    public CreateOrderHandler(AppDbContext context, IMapper mapper, IPaymentApi paymentApi, ILogger<CreateOrderHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _paymentApi = paymentApi;
        _logger = logger;
    }

    public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = _mapper.Map<Order>(request.Request);
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        var paymentRequest = new CreatePaymentRequest
        {
            OrderId = order.Id,
            Price = order.Price,
            EmailClient = order.EmailClient,
            PhoneNumber = order.PhoneNumber
        };
        try
        {
            var response = await _paymentApi.CreatePaymentAsync(paymentRequest);

            if (!response.Success)
            {
                _logger.LogWarning("Payment failed for order {OrderId}: {Message}", order.Id, response.Message);
                order.Status = "PaymentFailed";
                await _context.SaveChangesAsync(cancellationToken);
                throw new InvalidOperationException($"Payment service error: {response.Message}");
            }

            order.Status = "PaymentReserved";
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling PaymentService for order {OrderId}", order.Id);
            order.Status = "PaymentFailed";
            await _context.SaveChangesAsync(cancellationToken);
            throw;
        }

        return _mapper.Map<OrderResponse>(order);
    }
}