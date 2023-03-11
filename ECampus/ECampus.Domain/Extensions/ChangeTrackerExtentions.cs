using ECampus.Domain.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ECampus.Domain.Extensions;

public static class ChangeTrackerExtensions
{
    public static void UpdateDeleteStatus(this ChangeTracker changeTracker)
    {
        changeTracker.DetectChanges();

        var entities = changeTracker.Entries().Where(t => t.Entity is IIsDeleted && t.State == EntityState.Deleted).ToList();
        
        foreach (var entry in entities)
        {
            var entity = (IIsDeleted)entry.Entity;
            entity.IsDeleted = true;
            entry.State = EntityState.Unchanged;
        }
    }

}