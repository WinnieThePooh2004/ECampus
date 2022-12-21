using AutoFixture;

namespace UniversityTimetable.Tests.Shared.DataGeneration
{
    public class DataGenerator
    {
        public static TData Create<TData>()
        {
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            return fixture.Create<TData>();
        }
    }
}
