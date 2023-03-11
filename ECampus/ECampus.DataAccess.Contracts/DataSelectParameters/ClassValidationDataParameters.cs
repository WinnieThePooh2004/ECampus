using ECampus.Domain.Enums;
using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public class ClassValidationDataParameters : IDataSelectParameters<Class>
{
    public int Number { get; init; }
    public int DayOfWeek { get; init; }
    public WeekDependency WeekDependency { get; init; }
}