using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;

namespace UniversityTimetable.Domain.Services;

public class ServiceWithUpdateValidation<TDto> : IBaseService<TDto>
    where TDto : class, IDataTransferObject
{
    private readonly IBaseService<TDto> _baseService;
    private readonly IUpdateValidator<TDto> _validator;

    public ServiceWithUpdateValidation(IBaseService<TDto> baseService, IUpdateValidator<TDto> validator)
    {
        _baseService = baseService;
        _validator = validator;
    }

    public Task<TDto> GetByIdAsync(int? id) => _baseService.GetByIdAsync(id);

    public Task<TDto> CreateAsync(TDto entity) => _baseService.CreateAsync(entity);

    public async Task<TDto> UpdateAsync(TDto entity)
    {
        var errors = await _validator.ValidateAsync(entity);
        if (errors.Any())
        {
            throw new ValidationException(typeof(TDto), errors);
        }

        return await _baseService.UpdateAsync(entity);
    }

    public Task DeleteAsync(int? id) => _baseService.DeleteAsync(id);
}