using ECampus.Domain.Entities;
using ECampus.Domain.Requests;

namespace ECampus.DataAccess.Contracts.DataSelectParameters;

public readonly struct TeacherTimetableParameters : IDataSelectParameters<Class>
{
    public readonly int TeacherId;

    public TeacherTimetableParameters(int teacherId)
    {
        TeacherId = teacherId;
    }
}