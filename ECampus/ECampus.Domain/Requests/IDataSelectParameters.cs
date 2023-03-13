using ECampus.Domain.Data;

namespace ECampus.Domain.Requests;

// ReSharper disable once UnusedTypeParameter
public interface IDataSelectParameters<T>
    where T : class, IEntity
{
}