using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models;

namespace Migrations.ModelConfigurations
{
    public class FacultacyConfiguration : IEntityTypeConfiguration<Facultacy>
    {
        public void Configure(EntityTypeBuilder<Facultacy> builder)
        {
            builder.HasQueryFilter(f => !f.IsDeleted);
        }
    }
}
