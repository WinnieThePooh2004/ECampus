using ECampus.Domain.Data;

namespace ECampus.Domain.Commands;

public interface ICreateCommand
{
    
}

// ReSharper disable once UnusedTypeParameter
public interface ICreateCommand<TEntity> : ICreateCommand
    where TEntity : IEntity
{
    
}