using FluentValidation;
using MediatR;

namespace OrderService.WebApi.Pipeline;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            // Вариант: выбросить ValidationException, если невалидно
            await Task.WhenAll(_validators.Select(v => v.ValidateAndThrowAsync(request, cancellationToken)));
        }
        return await next();
    }
}