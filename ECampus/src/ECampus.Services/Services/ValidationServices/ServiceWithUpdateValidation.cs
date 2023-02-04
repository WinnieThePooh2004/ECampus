using ECampus.Contracts.Services;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.Data;
using ECampus.Shared.Exceptions.DomainExceptions;

namespace ECampus.Services.Services.ValidationServices;

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

    public Task<TDto> GetByIdAsync(int id) => _baseService.GetByIdAsync(id);

    public Task<TDto> CreateAsync(TDto entity) => _baseService.CreateAsync(entity);

    public async Task<TDto> UpdateAsync(TDto entity)
    {
        var errors = await _validator.ValidateAsync(entity);
        if (!errors.IsValid)
        {
            throw new ValidationException(typeof(TDto), errors);
        }

        return await _baseService.UpdateAsync(entity);
    }

    public Task<TDto> DeleteAsync(int id) => _baseService.DeleteAsync(id);
}