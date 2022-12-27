using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared.DataFactories;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.ItemSelectorsTests;

public sealed class SingleItemSelectorAuditoryTests : SingleItemSelectorTests<Auditory>, IClassFixture<AuditoryFactory>
{
    public SingleItemSelectorAuditoryTests(AuditoryFactory dataFactory) : base(dataFactory)
    {
    }

    [Fact] protected override Task GetById_ReturnsFromSet_IfModelExists() => base.GetById_ReturnsFromSet_IfModelExists();

    [Fact] protected override Task GetById_ReturnsNull_IfItemDoesNotExist() => base.GetById_ReturnsNull_IfItemDoesNotExist();
}