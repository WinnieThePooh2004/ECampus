using ECampus.Domain.Data;
using ECampus.Domain.Entities;
using ECampus.Domain.Metadata;

namespace ECampus.Domain.DataTransferObjects;

[Dto<Student>]
[Validation]
public class StudentDto : IDataTransferObject
{
    public int Id { get; set; }
    
    public string LastName { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;
    
    public int GroupId { get; set; }
    
    public string? UserEmail { get; set; }
}