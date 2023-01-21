using ECampus.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECampus.Infrastructure.ModelConfigurations;

public class CourseTaskConfigurations : IEntityTypeConfiguration<CourseTask>
{
    public void Configure(EntityTypeBuilder<CourseTask> builder)
    {
        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasOne(c => c.Course)
            .WithMany(c => c.Tasks)
            .HasForeignKey(c => c.CourseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}