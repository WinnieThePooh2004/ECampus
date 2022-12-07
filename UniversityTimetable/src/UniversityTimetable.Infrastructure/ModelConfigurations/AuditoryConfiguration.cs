using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.ModelConfigurations
{
    public class AuditoryConfiguration : IEntityTypeConfiguration<Auditory>
    {
        public void Configure(EntityTypeBuilder<Auditory> builder)
        {
            builder.HasQueryFilter(a => !a.IsDeleted);
        }
    }
}
