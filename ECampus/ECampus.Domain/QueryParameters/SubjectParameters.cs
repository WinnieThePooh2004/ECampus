using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Models;

namespace ECampus.Domain.QueryParameters;

public class SubjectParameters : QueryParameters<SubjectDto>, IDataSelectParameters<Subject>
{
    public string? Name { get; set; }
}