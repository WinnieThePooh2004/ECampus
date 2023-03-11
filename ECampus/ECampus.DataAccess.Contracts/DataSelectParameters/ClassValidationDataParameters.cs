using ECampus.Shared.Enums;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public class ClassValidationDataParameters : IDataSelectParameters<Class>
{
    public int Number { get; init; }
    public int DayOfWeek { get; init; }
    public WeekDependency WeekDependency { get; init; }
}