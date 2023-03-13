using ECampus.Domain.Data;

namespace ECampus.Domain.Commands;

public interface IUpdateCommand
{
    
}

// ReSharper disable once UnusedTypeParameter
public interface IUpdateCommand<TEntity> : IUpdateCommand
    where TEntity : IEntity
{
    
}