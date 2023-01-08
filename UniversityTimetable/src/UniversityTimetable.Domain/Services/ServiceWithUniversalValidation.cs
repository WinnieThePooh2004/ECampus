using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;

namespace UniversityTimetable.Domain.Services;

public class ServiceWithUniversalValidation<TDto> : IBaseService<TDto>
    where TDto : class, IDataTransferObject
{
    private readonly IBaseService<TDto> _baseService;
    private readonly IUniversalValidator<TDto> _validator;

    public ServiceWithUniversalValidation(IBaseService<TDto> baseService, IUniversalValidator<TDto> validator)
    {
        _baseService = baseService;
        _validator = validator;
    }

    public Task<TDto> GetByIdAsync(int? id) => _baseService.GetByIdAsync(id);

    public async Task<TDto> UpdateAsync(TDto entity)
    {
        var errors = await _validator.ValidateAsync(entity);
        if (errors.Any())
        {
            throw new ValidationException(typeof(TDto), errors);
        }

        return await _baseService.UpdateAsync(entity);
    }

    public async Task<TDto> CreateAsync(TDto entity)
    {
        var errors = await _validator.ValidateAsync(entity);
        if (errors.Any())
        {
            throw new ValidationException(typeof(TDto), errors);
        }

        return await _baseService.CreateAsync(entity);
    }

    public Task DeleteAsync(int? id) => _baseService.DeleteAsync(id);
}