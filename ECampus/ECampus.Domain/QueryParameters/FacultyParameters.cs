using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Entities;

namespace ECampus.Domain.QueryParameters;

public class FacultyParameters : QueryParameters<FacultyDto>, IDataSelectParameters<Faculty>
{
    public string? Name { get; set; }
}