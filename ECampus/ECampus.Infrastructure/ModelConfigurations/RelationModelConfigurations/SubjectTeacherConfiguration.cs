using ECampus.Shared.Models.RelationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECampus.Infrastructure.ModelConfigurations.RelationModelConfigurations;

public class SubjectTeacherConfiguration : IEntityTypeConfiguration<SubjectTeacher>
{
    public void Configure(EntityTypeBuilder<SubjectTeacher> builder)
    {
        builder.HasKey(s => new{ s.TeacherId, s.SubjectId });
            
        builder.HasOne(s => s.Teacher)
            .WithMany(s => s.SubjectIds)
            .HasForeignKey(s => s.TeacherId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Subject)
            .WithMany(s => s.TeacherIds)
            .HasForeignKey(s => s.SubjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}