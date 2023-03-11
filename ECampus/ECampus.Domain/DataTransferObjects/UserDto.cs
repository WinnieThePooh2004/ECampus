using ECampus.Domain.Data;
using ECampus.Domain.Enums;
using ECampus.Domain.Metadata;
using ECampus.Domain.Models;

namespace ECampus.Domain.DataTransferObjects;

[Dto<User>(InjectBaseService = false)]
[Validation]
public class UserDto : IDataTransferObject
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }

    public int? StudentId { get; set; }

    public int? TeacherId { get; set; }

    public TeacherDto? Teacher { get; set; }
    public StudentDto? Student { get; set; }
}