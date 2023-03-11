using ECampus.Domain.Enums;
using ECampus.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace ECampus.Infrastructure.ModelConfigurations;

public class TeacherRateConfiguration : IEntityTypeConfiguration<TeacherRate>
{
    public void Configure(EntityTypeBuilder<TeacherRate> builder)
    {
        builder.HasOne(t => t.Teacher)
            .WithMany(t => t.Rates)
            .HasForeignKey(t => t.TeacherId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Student)
            .WithMany(t => t.Rates)
            .HasForeignKey(t => t.StudentId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(t => t.Course)
            .WithMany(t => t.Rates)
            .HasForeignKey(t => t.CourseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(t => new { t.TeacherId, t.StudentId, t.CourseId });

        builder.Property(t => t.Rates)
            .HasMaxLength(1024)
            .HasColumnName("Rates")
            .HasConversion(rates => JsonConvert.SerializeObject(rates),
                rates => JsonConvert.DeserializeObject<Dictionary<RateType, int>>(rates)!);

        builder.Property(t => t.Feedback)
            .HasMaxLength(512);
    }
}