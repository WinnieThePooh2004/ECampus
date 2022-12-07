using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.General;

namespace UniversityTimetable.Shared.Extentions
{
    public static class ChangeTrackerExtentions
    {
        public static void UpdateDeleteStatus(this ChangeTracker changeTracker)
        {
            changeTracker.DetectChanges();

            var entities = changeTracker.Entries().Where(t => t.Entity is IIsDeleted && t.State == EntityState.Deleted);

            if (entities.Any())
            {
                foreach (var entry in entities)
                {
                    var entity = (IIsDeleted)entry.Entity;
                    entity.IsDeleted = true;
                    entry.State = EntityState.Unchanged;
                }
            }
        }

    }
}
