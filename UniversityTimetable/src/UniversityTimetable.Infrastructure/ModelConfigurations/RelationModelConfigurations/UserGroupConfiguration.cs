using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.ModelConfigurations.RelationModelConfigurations;

public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.HasQueryFilter(s => !s.IsDeleted)
            .HasOne(s => s.User)
            .WithMany(s => s.SavedGroupsIds)
            .HasForeignKey(s => s.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.Group)
            .WithMany(s => s.UsersIds)
            .HasForeignKey(s => s.GroupId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}