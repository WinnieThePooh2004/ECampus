using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Shared.Extensions;

public static class ChangeTrackerExtensions
{
    public static void UpdateDeleteStatus(this ChangeTracker changeTracker)
    {
        changeTracker.DetectChanges();

        var entities = changeTracker.Entries().Where(t => t.Entity is IIsDeleted && t.State == EntityState.Deleted).ToList();

        if (!entities.Any())
        {
            return;
        }
        foreach (var entry in entities)
        {
            var entity = (IIsDeleted)entry.Entity;
            entity.IsDeleted = true;
            entry.State = EntityState.Unchanged;
        }
    }

}