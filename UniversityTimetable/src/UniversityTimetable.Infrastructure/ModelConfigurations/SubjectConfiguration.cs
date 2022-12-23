using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Infrastructure.ModelConfigurations
{
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
}
