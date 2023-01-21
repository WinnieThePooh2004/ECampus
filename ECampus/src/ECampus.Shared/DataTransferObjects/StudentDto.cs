using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Metadata;
using ECampus.Shared.Models;

namespace ECampus.Shared.DataTransferObjects;

[Dto<Student>]
public class StudentDto : IDataTransferObject
{
    public int Id { get; set; }
    [DisplayName("Last name", 0)]
    public string LastName { get; set; } = string.Empty;
    [DisplayName("First name", 1)]
    public string FirstName { get; set; } = string.Empty;
    public int GroupId { get; set; }
    
    public int? UserId { get; set; }
}