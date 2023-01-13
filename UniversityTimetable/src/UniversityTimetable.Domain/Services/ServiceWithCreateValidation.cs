using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;

namespace UniversityTimetable.Domain.Services;

public class ServiceWithCreateValidation<TDto> : IBaseService<TDto>
    where TDto : class, IDataTransferObject
{
    private readonly IBaseService<TDto> _baseService;
    private readonly ICreateValidator<TDto> _validator;

    public ServiceWithCreateValidation(IBaseService<TDto> baseService, ICreateValidator<TDto> validator)
    {
        _baseService = baseService;
        _validator = validator;
    }

    public Task<TDto> GetByIdAsync(int? id) => _baseService.GetByIdAsync(id);

    public Task<TDto> UpdateAsync(TDto entity) => _baseService.UpdateAsync(entity);

    public async Task<TDto> CreateAsync(TDto entity)
    {
        var errors = await _validator.ValidateAsync(entity);
        if (errors.Any())
        {
            throw new ValidationException(typeof(TDto), errors);
        }

        return await _baseService.CreateAsync(entity);
    }

    public Task<TDto> DeleteAsync(int? id) => _baseService.DeleteAsync(id);
}