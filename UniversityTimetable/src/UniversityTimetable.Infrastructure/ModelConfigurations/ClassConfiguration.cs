using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Infrastructure.ModelConfigurations
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasQueryFilter(c => !c.IsDeleted);

            builder.HasOne(c => c.Teacher)
                .WithMany(t => t.Classes)
                .HasForeignKey(c => c.TeacherId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Group)
                .WithMany(g => g.Classes)
                .HasForeignKey(c => c.GroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Auditory)
                .WithMany(t => t.Classes)
                .HasForeignKey(c => c.AuditoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Subject)
                .WithMany(t => t.Classes)
                .HasForeignKey(c => c.SubjectId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
