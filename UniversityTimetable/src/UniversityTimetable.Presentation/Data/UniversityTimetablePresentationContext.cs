using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Presentation.Data
{
    public class UniversityTimetablePresentationContext : DbContext
    {
        public UniversityTimetablePresentationContext (DbContextOptions<UniversityTimetablePresentationContext> options)
            : base(options)
        {
        }

        public DbSet<UniversityTimetable.Shared.Models.Auditory> Auditory { get; set; } = default!;

        public DbSet<UniversityTimetable.Shared.Models.Class> Class { get; set; } = default!;

        public DbSet<UniversityTimetable.Shared.DataTransferObjects.TeacherDTO> TeacherDTO { get; set; } = default!;

        public DbSet<UniversityTimetable.Shared.DataTransferObjects.GroupDTO> GroupDTO { get; set; } = default!;
    }
}
