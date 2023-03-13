using ECampus.Domain.Responses.Subject;

namespace ECampus.Domain.Requests.Subject;

public class SubjectParameters : QueryParameters<MultipleSubjectResponse>, IDataSelectParameters<Entities.Subject>
{
    public string? Name { get; set; }
}