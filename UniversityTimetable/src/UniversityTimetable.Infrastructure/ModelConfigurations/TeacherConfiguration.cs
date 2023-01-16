using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.ModelConfigurations;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.HasQueryFilter(t => !t.IsDeleted)
            .HasOne(t => t.Department)
            .WithMany(d => d.Teachers)
            .HasForeignKey(t => t.DepartmentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Subjects)
            .WithMany(s => s.Teachers)
            .UsingEntity<SubjectTeacher>();
        
        builder.HasOne(t => t.User)
            .WithOne(u => u.Teacher)
            .HasForeignKey<User>(u => u.StudentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}