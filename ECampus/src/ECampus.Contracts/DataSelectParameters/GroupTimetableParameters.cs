using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;

namespace ECampus.Contracts.DataSelectParameters;

public class GroupTimetableParameters : IDataSelectParameters<Class>
{
    public int GroupId { get; set; }
}