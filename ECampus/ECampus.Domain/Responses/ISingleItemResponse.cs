using ECampus.Domain.Data;

namespace ECampus.Domain.Responses;

public interface ISingleItemResponse
{
    int Id { get; set; }
}

// ReSharper disable once UnusedTypeParameter
public interface ISingleItemResponse<TEntity> : ISingleItemResponse
    where TEntity : IEntity
{
    
}