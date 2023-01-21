using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.ModelConfigurations;

public class CourseTaskConfigurations : IEntityTypeConfiguration<CourseTask>
{
    public void Configure(EntityTypeBuilder<CourseTask> builder)
    {
        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasOne(c => c.Course)
            .WithMany(c => c.Tasks)
            .HasForeignKey(c => c.CourseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}