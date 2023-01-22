﻿using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class StudentParameters : QueryParameters, IQueryParameters<Student>
{
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public int GroupId { get; set; }

    public bool UserIdCanBeNull { get; set; } = true;
}