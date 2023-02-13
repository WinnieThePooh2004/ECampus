using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Shared.QueryParameters;

public class FacultyParameters : QueryParameters<FacultyDto>, IDataSelectParameters<Faculty>
{
    public string? Name { get; set; }
}