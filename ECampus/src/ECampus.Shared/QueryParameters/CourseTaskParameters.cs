﻿using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class CourseTaskParameters : QueryParameters, IDataSelectParameters<CourseTask>
{
    public int CourseId { get; set; }
}