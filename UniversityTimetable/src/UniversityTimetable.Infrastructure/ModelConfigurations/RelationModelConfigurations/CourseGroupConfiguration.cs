using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.ModelConfigurations.RelationModelConfigurations;

public class CourseGroupConfiguration : IEntityTypeConfiguration<CourseGroup>
{
    public void Configure(EntityTypeBuilder<CourseGroup> builder)
    {
        builder.HasKey(c => new { c.CourseId, c.GroupId });

        builder.HasOne(c => c.Group)
            .WithMany(g => g.CourseGroups)
            .HasForeignKey(g => g.GroupId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.Course)
            .WithMany(c => c.CourseGroups)
            .HasForeignKey(g => g.CourseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}