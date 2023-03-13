using ECampus.Domain.Data;

namespace ECampus.Domain.Responses;

public interface IMultipleItemsResponse
{
    int Id { get; set; }
}

// ReSharper disable once UnusedTypeParameter
public interface IMultipleItemsResponse<TEntity> : IMultipleItemsResponse
    where TEntity : IEntity
{
    
}