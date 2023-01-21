using AutoFixture;

namespace ECampus.Tests.Shared.DataFactories;

public interface IAbstractFactory<out T>
{
    public T CreateModel(Fixture fixture);
}