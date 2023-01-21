using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.ModelConfigurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasMany(c => c.Groups)
            .WithMany(g => g.Courses)
            .UsingEntity<CourseGroup>();

        builder.HasMany(c => c.Teachers)
            .WithMany(t => t.Courses)
            .UsingEntity<CourseTeacher>();
    }
}