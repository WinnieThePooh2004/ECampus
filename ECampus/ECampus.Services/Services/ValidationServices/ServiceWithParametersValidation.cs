using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Domain.Requests;
using ECampus.Domain.Responses;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Contracts.Validation;

namespace ECampus.Services.Services.ValidationServices;

public class ServiceWithParametersValidation<TResponse, TParameters> : IParametersService<TResponse, TParameters>
    where TParameters : IQueryParameters<TResponse>
    where TResponse : class, IMultipleItemsResponse
{
    private readonly IParametersService<TResponse, TParameters> _baseService;
    private readonly IParametersValidator<TParameters> _parametersValidator;

    public ServiceWithParametersValidation(IParametersService<TResponse, TParameters> baseService,
        IParametersValidator<TParameters> parametersValidator)
    {
        _baseService = baseService;
        _parametersValidator = parametersValidator;
    }

    public async Task<ListWithPaginationData<TResponse>> GetByParametersAsync(TParameters parameters, CancellationToken token = default)
    {
        var errors = await _parametersValidator.ValidateAsync(parameters, token);
        if (!errors.IsValid)
        {
            throw new ValidationException(typeof(TParameters), errors);
        }

        return await _baseService.GetByParametersAsync(parameters, token);
    }
}