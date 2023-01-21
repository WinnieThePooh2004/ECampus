using AutoFixture;
using ECampus.Tests.Shared.DataFactories;

namespace ECampus.Tests.Shared.Extensions;

public static class AbstractFactoryExtensions
{
    public static List<T> CreateMany<T>(this IAbstractFactory<T> factory, Fixture fixture, int amount)
    {
        return Enumerable.Range(0, amount).Select(_ =>factory.CreateModel(fixture)).ToList();
    }
    
}