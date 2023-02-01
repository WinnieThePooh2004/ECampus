using ECampus.Shared.Data;

namespace ECampus.Shared.Models
{
    public class Faculty : IIsDeleted, IModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;        
        public bool IsDeleted { get; set; }
        public List<Department>? Departments { get; set; }
    }
}
