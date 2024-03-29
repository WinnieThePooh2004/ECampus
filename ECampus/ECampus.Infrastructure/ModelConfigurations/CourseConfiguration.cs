﻿using ECampus.Domain.Entities;
using ECampus.Domain.Entities.RelationEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECampus.Infrastructure.ModelConfigurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasMany(c => c.Groups)
            .WithMany(g => g.Courses)
            .UsingEntity<CourseGroup>();

        builder.HasMany(c => c.Teachers)
            .WithMany(t => t.Courses)
            .UsingEntity<CourseTeacher>();

        builder.HasOne(c => c.Subject)
            .WithMany(s => s.Courses)
            .HasForeignKey(c => c.SubjectId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}