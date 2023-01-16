using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.ModelConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasQueryFilter(u => !u.IsDeleted)
            .HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.HasOne(u => u.Teacher)
            .WithOne(t => t.User)
            .HasForeignKey<Teacher>(t => t.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(u => u.Student)
            .WithOne(s => s.User)
            .HasForeignKey<Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.SavedAuditories)
            .WithMany(a => a.Users)
            .UsingEntity<UserAuditory>();

        builder.HasMany(u => u.SavedGroups)
            .WithMany(g => g.Users)
            .UsingEntity<UserGroup>();

        builder.HasMany(u => u.SavedTeachers)
            .WithMany(t => t.Users)
            .UsingEntity<UserTeacher>();
    }
}