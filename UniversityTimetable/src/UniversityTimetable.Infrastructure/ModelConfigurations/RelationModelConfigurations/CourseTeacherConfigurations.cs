using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.ModelConfigurations.RelationModelConfigurations;

public class CourseTeacherConfigurations : IEntityTypeConfiguration<CourseTeacher>
{
    public void Configure(EntityTypeBuilder<CourseTeacher> builder)
    {
        builder.HasKey(c => new { c.CourseId, c.TeacherId });

        builder.HasOne(c => c.Teacher)
            .WithMany(g => g.CourseTeachers)
            .HasForeignKey(g => g.TeacherId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Course)
            .WithMany(c => c.CourseTeachers)
            .HasForeignKey(g => g.CourseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}