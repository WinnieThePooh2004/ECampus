using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataDeleteTests;

public sealed class DeleteDeleteAuditoryTests : DataDeleteTests<Auditory>
{
    [Fact]  protected override Task Delete_ModelRemovedFromContext() => base.Delete_ModelRemovedFromContext();
}