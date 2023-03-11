using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct TeacherTimetableParameters : IDataSelectParameters<Class>
{
    public readonly int TeacherId;

    public TeacherTimetableParameters(int teacherId)
    {
        TeacherId = teacherId;
    }
}