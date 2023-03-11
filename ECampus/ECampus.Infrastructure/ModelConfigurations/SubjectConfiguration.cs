using ECampus.Domain.Entities;
using ECampus.Domain.Entities.RelationEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECampus.Infrastructure.ModelConfigurations;

public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasQueryFilter(s => !s.IsDeleted);

        builder.HasMany(s => s.Teachers)
            .WithMany(s => s.Subjects)
            .UsingEntity<SubjectTeacher>();

    }
}