using ECampus.Domain.Data;

namespace ECampus.Domain.Entities;

public class Department : IIsDeleted, IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsDeleted { get; set; }

    public int FacultyId { get; set; }
    public Faculty? Faculty { get; set; }
    public List<Teacher>? Teachers { get; set; }
    public List<Group>? Groups { get; set; }
}