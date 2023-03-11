﻿using ECampus.Domain.Models;
using ECampus.Domain.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public readonly struct TeacherTimetableParameters : IDataSelectParameters<Class>
{
    public readonly int TeacherId;

    public TeacherTimetableParameters(int teacherId)
    {
        TeacherId = teacherId;
    }
}