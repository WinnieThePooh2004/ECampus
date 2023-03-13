using ECampus.Domain.Data;

namespace ECampus.Services.Interfaces;

// ReSharper disable once UnusedTypeParameter
public interface IDeleteHandler<TEntity>
    where TEntity : class, IEntity
{
    public Task DeleteAsync(int id, CancellationToken token);
}