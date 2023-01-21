using ECampus.Shared.Models.RelationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECampus.Infrastructure.ModelConfigurations.RelationModelConfigurations;

public class UserGroupConfiguration : IEntityTypeConfiguration<UserGroup>
{
    public void Configure(EntityTypeBuilder<UserGroup> builder)
    {
        builder.HasKey(s => new{ s.UserId, s.GroupId });
        builder.ToTable("UserGroups");

        builder.HasOne(s => s.User)
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