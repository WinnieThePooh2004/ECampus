using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Domain.Requests;
using ECampus.Domain.Responses;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Contracts.Validation;

namespace ECampus.Services.Services.ValidationServices;

public class HandlerWithGetByParametersValidation<TResponse, TParameters> : IGetByParametersHandler<TResponse, TParameters>
    where TParameters : IQueryParameters<TResponse>
    where TResponse : class, IMultipleItemsResponse
{
    private readonly IGetByParametersHandler<TResponse, TParameters> _baseHandler;
    private readonly IParametersValidator<TParameters> _parametersValidator;

    public HandlerWithGetByParametersValidation(IGetByParametersHandler<TResponse, TParameters> baseHandler,
        IParametersValidator<TParameters> parametersValidator)
    {
        _baseHandler = baseHandler;
        _parametersValidator = parametersValidator;
    }

    public async Task<ListWithPaginationData<TResponse>> GetByParametersAsync(TParameters parameters, CancellationToken token = default)
    {
        var errors = await _parametersValidator.ValidateAsync(parameters, token);
        if (!errors.IsValid)
        {
            throw new ValidationException(typeof(TParameters), errors);
        }

        return await _baseHandler.GetByParametersAsync(parameters, token);
    }
}