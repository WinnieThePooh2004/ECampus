using AutoFixture;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Tests.Shared.DataFactories;

public interface IAbstractFactory<out T>
{
    public abstract T CreateModel(Fixture fixture);
}