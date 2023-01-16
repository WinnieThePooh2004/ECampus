using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.ModelConfigurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasQueryFilter(s => !s.IsDeleted)
            .HasOne(s => s.Group)
            .WithMany(g => g.Students)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.User)
            .WithOne(u => u.Student)
            .HasForeignKey<User>(u => u.StudentId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}