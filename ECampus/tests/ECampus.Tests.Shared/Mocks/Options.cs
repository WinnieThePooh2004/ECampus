using Microsoft.Extensions.Options;

namespace ECampus.Tests.Shared.Mocks;

public class Options<T> : IOptions<T> where T : class
{
    public T Value { get; }

    public Options(T value)
    {
        Value = value;
    }
}