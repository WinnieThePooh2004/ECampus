using ECampus.Domain.Data;

namespace ECampus.Domain.Responses;

public interface ISingleItemResponse
{
    int Id { get; set; }
}

public interface ISingleItemResponse<TEntity> : ISingleItemResponse
    where TEntity : IEntity
{
    
}