using Microsoft.Extensions.Options;

namespace ECampus.Tests.Shared.Mocks;

public class Options<T> : IOptions<T> where T : class
{
    public required T Value { get; init; }
}