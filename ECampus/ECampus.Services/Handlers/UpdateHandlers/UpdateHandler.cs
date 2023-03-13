using AutoMapper;
using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.Commands;
using ECampus.Domain.Data;
using ECampus.Services.Interfaces;

namespace ECampus.Services.Handlers.UpdateHandlers;

public class UpdateHandler<TUpdateCommand, TEntity> : IUpdateHandler<TUpdateCommand>
    where TUpdateCommand : IUpdateCommand<TEntity>
    where TEntity : class, IEntity
{
    private readonly IMapper _mapper;
    private readonly IDataAccessFacade _dataAccess;

    public UpdateHandler(IMapper mapper, IDataAccessFacade dataAccess)
    {
        _mapper = mapper;
        _dataAccess = dataAccess;
    }

    public async Task UpdateAsync(TUpdateCommand updateCommand, CancellationToken token)
    {
        var entity = _mapper.Map<TEntity>(updateCommand);
        await _dataAccess.UpdateAsync(entity, token);
        await _dataAccess.SaveChangesAsync(token);
    }
}