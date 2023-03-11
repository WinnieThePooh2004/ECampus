using ECampus.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECampus.Infrastructure.ModelConfigurations;

public class TaskSubmissionConfiguration : IEntityTypeConfiguration<TaskSubmission>
{
    public void Configure(EntityTypeBuilder<TaskSubmission> builder)
    {
        builder.HasIndex(t => new { t.StudentId, t.CourseTaskId })
            .IsUnique();

        builder.Property(t => t.SubmissionContent).HasMaxLength(450);

        builder.HasOne(t => t.Student)
            .WithMany(s => s.Submissions)
            .HasForeignKey(t => t.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.CourseTask)
            .WithMany(t => t.Submissions)
            .HasForeignKey(t => t.CourseTaskId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}