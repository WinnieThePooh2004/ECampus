using ECampus.Shared.Interfaces.Data.Models;
using ECampus.Shared.Models.RelationModels;

namespace ECampus.Shared.Models;

public class Auditory : IIsDeleted, IModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;

    public List<Class>? Classes { get; set; }
    public bool IsDeleted { get; set; }
    public List<User>? Users { get; set; }
    public List<UserAuditory>? UsersIds { get; set; }
}