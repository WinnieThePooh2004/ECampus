using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.ModelConfigurations.RelationModelConfigurations;

public class UserTeacherConfiguration : IEntityTypeConfiguration<UserTeacher>
{
    public void Configure(EntityTypeBuilder<UserTeacher> builder)
    {
        builder.HasKey(s => new{ s.UserId, s.TeacherId });
        builder.ToTable("UserTeachers");

        builder.HasOne(s => s.User)
            .WithMany(s => s.SavedTeachersIds)
            .HasForeignKey(s => s.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Teacher)
            .WithMany(s => s.UsersIds)
            .HasForeignKey(s => s.TeacherId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

    }
}