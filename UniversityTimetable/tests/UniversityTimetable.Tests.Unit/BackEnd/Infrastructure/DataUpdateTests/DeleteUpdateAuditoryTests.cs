using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataUpdateTests;

public sealed class DeleteUpdateAuditoryTests : DataUpdateTests<Auditory>
{
    [Fact]  protected override Task Update_ModelRemovedFromContext() => base.Update_ModelRemovedFromContext();
}