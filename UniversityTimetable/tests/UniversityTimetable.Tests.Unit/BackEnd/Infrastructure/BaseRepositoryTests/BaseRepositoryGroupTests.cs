using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared.DataFactories;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.BaseRepositoryTests;

public class BaseRepositoryGroupTests : BaseRepositoryTests<Group>, IClassFixture<GroupFactory>
{
    [Fact] protected override Task Create_AddedToDb() => base.Create_AddedToDb();

    [Fact] protected override Task Create_ShouldThrowException_WhenModelIdNot0() => base.Create_ShouldThrowException_WhenModelIdNot0();

    [Fact] protected override Task Update_UpdatedInDb_IfExistsInDb() => base.Update_UpdatedInDb_IfExistsInDb();

    [Fact] protected override Task Update_ShouldThrowException_IfSaveChangeThrowsException() => base.Update_ShouldThrowException_IfSaveChangeThrowsException();

    [Fact] protected override Task GetById_ReturnsFromDb_IfExistsInDb() => base.GetById_ReturnsFromDb_IfExistsInDb();

    [Fact] protected override Task GetByIdAsync_ShouldThrowException_WhenSetReturnsNull() => base.GetByIdAsync_ShouldThrowException_WhenSetReturnsNull();

    public BaseRepositoryGroupTests(GroupFactory dataFactory) : base(dataFactory)
    {
    }
}