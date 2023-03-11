using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECampus.Infrastructure.ModelConfigurations;

public class AuditoryConfiguration : IEntityTypeConfiguration<Auditory>
{
    public void Configure(EntityTypeBuilder<Auditory> builder)
    {
        builder.HasQueryFilter(a => !a.IsDeleted);
    }
}