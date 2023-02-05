using ECampus.Shared.Enums;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public class ClassValidationDataParameters : IDataSelectParameters<Class>
{
    public int Number { get; set; }
    public int DayOfWeek { get; set; }
    public WeekDependency WeekDependency { get; set; }
}