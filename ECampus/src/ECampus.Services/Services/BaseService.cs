using AutoMapper;
using ECampus.Contracts.DataAccess;
using ECampus.Contracts.Services;
using ECampus.Shared.Data;
using ECampus.Shared.Exceptions.DomainExceptions;

namespace ECampus.Services.Services;

public class BaseService<TDto, TRepositoryModel> : IBaseService<TDto>
    where TRepositoryModel : class, IModel
    where TDto : class, IDataTransferObject
{
    private readonly IBaseDataAccessFacade<TRepositoryModel> _dataAccessFacade;
    private readonly IMapper _mapper;

    public BaseService(IBaseDataAccessFacade<TRepositoryModel> dataAccessFacade,
        IMapper mapper)
    {
        _dataAccessFacade = dataAccessFacade;
        _mapper = mapper;
    }

    public async Task<TDto> CreateAsync(TDto entity)
    {
        var auditory = _mapper.Map<TRepositoryModel>(entity);
        return _mapper.Map<TDto>(await _dataAccessFacade.CreateAsync(auditory));
    }

    public async Task<TDto> DeleteAsync(int? id)
    {
        if (id is null)
        {
            throw new NullIdException();
        }
        return _mapper.Map<TDto>(await _dataAccessFacade.DeleteAsync((int)id));
    }

    public async Task<TDto> GetByIdAsync(int? id)
    {
        if (id is null)
        {
            throw new NullIdException();
        }
        return _mapper.Map<TDto>(await _dataAccessFacade.GetByIdAsync((int)id));
    }

    public async Task<TDto> UpdateAsync(TDto entity)
    {
        var auditory = _mapper.Map<TRepositoryModel>(entity);
        return _mapper.Map<TDto>(await _dataAccessFacade.UpdateAsync(auditory));
    }
}