using ECampus.Domain.Metadata;

namespace ECampus.Domain.DataTransferObjects;

[Validation(decorateServices: false)]
public class PasswordChangeDto
{
    public int UserId { get; set; }
    public string OldPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string NewPasswordConfirm { get; set; } = string.Empty;
}