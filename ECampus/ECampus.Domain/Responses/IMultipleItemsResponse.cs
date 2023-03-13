using ECampus.Domain.Data;

namespace ECampus.Domain.Responses;

public interface IMultipleItemsResponse
{
    int Id { get; set; }
}

public interface IMultipleItemsResponse<TEntity> : IMultipleItemsResponse
    where TEntity : IEntity
{
    
}