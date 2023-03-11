using ECampus.Shared.Models.RelationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECampus.Infrastructure.ModelConfigurations.RelationModelConfigurations;

public class UserAuditoryConfiguration : IEntityTypeConfiguration<UserAuditory>
{
    public void Configure(EntityTypeBuilder<UserAuditory> builder)
    {
        builder.ToTable("UserAuditories");
        builder.HasKey(s => new{ s.UserId, s.AuditoryId });

        builder.HasOne(s => s.User)
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