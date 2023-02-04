using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public class TeacherTimetableParameters : IDataSelectParameters<Class>
{
    public int TeacherId { get; set; }
}