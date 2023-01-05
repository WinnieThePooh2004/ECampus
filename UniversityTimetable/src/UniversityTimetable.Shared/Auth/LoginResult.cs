using UniversityTimetable.Shared.Enums;

namespace UniversityTimetable.Shared.Auth;

public class LoginResult
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}