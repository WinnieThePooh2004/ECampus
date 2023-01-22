﻿using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class TeacherParameters : QueryParameters, IQueryParameters<Teacher>
{
    public int DepartmentId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public bool UserIdCanBeNull { get; set; } = true;
}