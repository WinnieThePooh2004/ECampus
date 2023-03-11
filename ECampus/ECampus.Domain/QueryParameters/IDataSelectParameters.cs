using ECampus.Domain.Data;

namespace ECampus.Domain.QueryParameters;

// ReSharper disable once UnusedTypeParameter
public interface IDataSelectParameters<T>
    where T : class, IModel
{
}