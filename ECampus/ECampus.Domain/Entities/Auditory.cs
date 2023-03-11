using ECampus.Domain.Data;
using ECampus.Domain.Entities.RelationEntities;

namespace ECampus.Domain.Entities;

public class Auditory : IIsDeleted, IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;

    public List<Class>? Classes { get; set; }
    public bool IsDeleted { get; set; }
    public List<User>? Users { get; set; }
    public List<UserAuditory>? UsersIds { get; set; }
}