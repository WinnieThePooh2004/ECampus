﻿using ECampus.Domain.Data;
using ECampus.Domain.DataContainers;
using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Domain.QueryParameters;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Contracts.Validation;

namespace ECampus.Services.Services.ValidationServices;

public class ServiceWithParametersValidation<TDto, TParameters> : IParametersService<TDto, TParameters>
    where TParameters : IQueryParameters<TDto>
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

    public async Task<ListWithPaginationData<TDto>> GetByParametersAsync(TParameters parameters, CancellationToken token = default)
    {
        var errors = await _parametersValidator.ValidateAsync(parameters, token);
        if (!errors.IsValid)
        {
            throw new ValidationException(typeof(TParameters), errors);
        }

        return await _baseService.GetByParametersAsync(parameters, token);
    }
}