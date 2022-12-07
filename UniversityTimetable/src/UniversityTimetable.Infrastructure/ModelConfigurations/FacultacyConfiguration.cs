using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityTimetable.Shared.Models;

namespace Migrations.ModelConfigurations
{
    public class FacultacyConfiguration : IEntityTypeConfiguration<Facultacy>
    {
        public void Configure(EntityTypeBuilder<Facultacy> builder)
        {
            builder.HasQueryFilter(f => !f.IsDeleted);
        }
    }
}
