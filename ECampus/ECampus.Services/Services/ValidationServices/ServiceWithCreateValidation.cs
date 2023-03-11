using ECampus.Domain.Data;
using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Contracts.Validation;

namespace ECampus.Services.Services.ValidationServices;

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

    public Task<TDto> GetByIdAsync(int id, CancellationToken token = default) => _baseService.GetByIdAsync(id, token);

    public Task<TDto> UpdateAsync(TDto dto, CancellationToken token = default) => _baseService.UpdateAsync(dto, token);

    public async Task<TDto> CreateAsync(TDto dto, CancellationToken token = default)
    {
        var errors = await _validator.ValidateAsync(dto, token);
        if (!errors.IsValid)
        {
            throw new ValidationException(typeof(TDto), errors);
        }

        return await _baseService.CreateAsync(dto, token);
    }

    public Task<TDto> DeleteAsync(int id, CancellationToken token = default) => _baseService.DeleteAsync(id, token);
}