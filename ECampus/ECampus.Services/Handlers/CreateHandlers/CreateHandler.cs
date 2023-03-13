using AutoMapper;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.Commands;
using ECampus.Domain.Data;
using ECampus.Domain.Responses;
using ECampus.Services.Interfaces;

namespace ECampus.Services.Handlers.CreateHandlers;

public class CreateHandler<TCreateCommand, TEntity> : ICreateHandler<TCreateCommand>
    where TCreateCommand : class, ICreateCommand<TEntity> 
    where TEntity : class, IEntity
{
    private readonly IMapper _mapper;
    private readonly IDataAccessFacade _dataAccess;

    public CreateHandler(IMapper mapper, IDataAccessFacade dataAccess)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
    }

    public async Task<CreatedResponse> CreateAsync(TCreateCommand createCommand, CancellationToken token)
    {
        var entity = _mapper.Map<TEntity>(createCommand);
        _dataAccess.Create(entity);
        await _dataAccess.SaveChangesAsync(token);
        return new CreatedResponse { Id = entity.Id };
    }
}