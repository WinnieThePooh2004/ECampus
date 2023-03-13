using ECampus.Domain.Entities;
using ECampus.Domain.Enums;
using ECampus.Domain.Requests;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public class ClassValidationDataParameters : IDataSelectParameters<Class>
{
    public int Number { get; init; }
    public int DayOfWeek { get; init; }
    public WeekDependency WeekDependency { get; init; }
}