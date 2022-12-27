using AutoFixture;
using UniversityTimetable.Tests.Shared.DataFactories;

namespace UniversityTimetable.Tests.Shared.Extensions;

public static class AbstractFactoryExtensions
{
    public static List<T> CreateMany<T>(this IAbstractFactory<T> factory, Fixture fixture, int amount)
    {
        return Enumerable.Range(0, amount).Select(i =>factory.CreateModel(fixture)).ToList();
    }
    
}