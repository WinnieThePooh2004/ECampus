using UniversityTimetable.Shared.Attributes;

namespace UniversityTimetable.Shared.DataTransferObjects;

[Validation(ValidationTypes.UpdateOnly, false, false)]
public class PasswordChangeDto
{
    public int UserId { get; set; }
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string NewPasswordConfirm { get; set; } = string.Empty;
}