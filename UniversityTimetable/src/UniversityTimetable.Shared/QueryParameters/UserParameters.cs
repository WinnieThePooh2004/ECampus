using UniversityTimetable.Shared.Enums;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Shared.QueryParameters;

public class UserParameters : QueryParameters, IQueryParameters<User>
{
    public string? Email { get; set; }
    public UserRole Role { get; set; } = UserRole.Guest;
    public string? Username { get; set; }
}