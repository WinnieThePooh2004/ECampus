﻿using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class SubjectParameters : QueryParameters<SubjectDto>, IDataSelectParameters<Subject>
{
    public string? Name { get; set; }
}