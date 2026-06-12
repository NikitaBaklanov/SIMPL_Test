using FluentValidation;

namespace OrderService.WebApi.UseCases.Orders.CreateOrder;

/// <summary>
/// Валидатор команды создания заказа.
/// Проверяет корректность всех полей запроса.
/// </summary>
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Request.ProductId).GreaterThan(0);
        RuleFor(x => x.Request.Amount).InclusiveBetween(1, 10000);
        RuleFor(x => x.Request.EmailClient).NotEmpty().EmailAddress();
        RuleFor(x => x.Request.Price).GreaterThan(0);
        RuleFor(x => x.Request.PhoneNumber).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$");
    }
}