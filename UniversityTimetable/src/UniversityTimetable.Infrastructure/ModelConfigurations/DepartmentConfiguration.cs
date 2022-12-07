using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.ModelConfigurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasQueryFilter(d => !d.IsDeleted)
                .HasOne(d => d.Facultacy)
                .WithMany(f => f.Departments)
                .HasForeignKey(f => f.FacultacyId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
