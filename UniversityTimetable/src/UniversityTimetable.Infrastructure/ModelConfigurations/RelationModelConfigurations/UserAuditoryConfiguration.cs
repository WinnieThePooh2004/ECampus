using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.ModelConfigurations.RelationModelConfigurations
{
    public class UserAuditoryConfiguration : IEntityTypeConfiguration<UserAuditory>
    {
        public void Configure(EntityTypeBuilder<UserAuditory> builder)
        {
            builder.HasQueryFilter(s => !s.IsDeleted)
                .HasOne(s => s.User)
                .WithMany(s => s.SavedAuditoriesIds)
                .HasForeignKey(s => s.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.Auditory)
                .WithMany(s => s.UsersIds)
                .HasForeignKey(s => s.AuditoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
