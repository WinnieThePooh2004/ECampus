using ECampus.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECampus.Infrastructure.ModelConfigurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasQueryFilter(d => !d.IsDeleted)
            .HasOne(d => d.Faculty)
            .WithMany(f => f.Departments)
            .HasForeignKey(f => f.FacultyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}