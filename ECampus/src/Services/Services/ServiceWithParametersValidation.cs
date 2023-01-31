using ECampus.Shared.DataContainers;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Interfaces.Domain;
using ECampus.Shared.Interfaces.Domain.Validation;
using ECampus.Shared.QueryParameters;

namespace Services.Services;

public class ServiceWithParametersValidation<TDto, TParameters> : IParametersService<TDto, TParameters>
    where TParameters : IQueryParameters
    where TDto : class, IDataTransferObject
{
    private readonly IParametersService<TDto, TParameters> _baseService;
    private readonly IParametersValidator<TParameters> _parametersValidator;

    public ServiceWithParametersValidation(IParametersService<TDto, TParameters> baseService,
        IParametersValidator<TParameters> parametersValidator)
    {
        _baseService = baseService;
        _parametersValidator = parametersValidator;
    }

    public async Task<ListWithPaginationData<TDto>> GetByParametersAsync(TParameters parameters)
    {
        var errors = await _parametersValidator.ValidateAsync(parameters);
        if (!errors.IsValid)
        {
            throw new ValidationException(typeof(TParameters), errors);
        }

        return await _baseService.GetByParametersAsync(parameters);
    }
}