namespace UniversityTimetable.Shared.DataTransferObjects;

public class PasswordChangeDto
{
    public int UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordConfirm { get; set; }
}