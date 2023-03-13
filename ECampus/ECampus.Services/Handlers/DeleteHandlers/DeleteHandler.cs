using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.Data;
using ECampus.Services.Interfaces;

namespace ECampus.Services.Handlers.DeleteHandlers;

public class DeleteHandler<TEntity> : IDeleteHandler<TEntity>
    where TEntity : class, IEntity, new()
{
    private readonly IDataAccessFacade _dataAccess;

    public DeleteHandler(IDataAccessFacade dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task DeleteAsync(int id, CancellationToken token)
    {
        var entity = new TEntity { Id = id };
        _dataAccess.Delete(entity);
        await _dataAccess.SaveChangesAsync(token);
    }
}