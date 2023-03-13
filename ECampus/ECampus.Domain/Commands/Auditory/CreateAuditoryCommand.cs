using System.ComponentModel.DataAnnotations;

namespace ECampus.Domain.Commands.Auditory;

public class CreateAuditoryCommand : ICreateCommand<Entities.Auditory>
{
    [Required]
    [StringLength(10)]
    public string Name { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string Building { get; set; } = default!;
}