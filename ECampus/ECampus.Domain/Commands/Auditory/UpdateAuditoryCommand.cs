using System.ComponentModel.DataAnnotations;

namespace ECampus.Domain.Commands.Auditory;

public class UpdateAuditoryCommand : IUpdateCommand<Entities.Auditory>
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(10)]
    public string Name { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string Building { get; set; } = default!;
}