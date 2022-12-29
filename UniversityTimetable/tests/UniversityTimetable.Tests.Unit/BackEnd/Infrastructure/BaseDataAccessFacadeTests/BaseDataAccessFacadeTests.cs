using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared.DataFactories;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.BaseDataAccessFacadeTests;

public sealed class BaseDataAccessFacadeTests : BaseDataAccessFacadeTests<Department>, IClassFixture<DepartmentFactory>
{
    [Fact] protected override Task Create_AddedToDb_CreateCalled() => base.Create_AddedToDb_CreateCalled();
    
    [Fact] protected override Task Update_UpdatedInDb_IfExistsInDb() => base.Update_UpdatedInDb_IfExistsInDb();

    [Fact] protected override Task Update_ShouldThrowException_IfSaveChangeThrowsException() => base.Update_ShouldThrowException_IfSaveChangeThrowsException();

    [Fact] protected override Task GetById_ReturnsFromSelector_IfSelectorReturnsItem() => base.GetById_ReturnsFromSelector_IfSelectorReturnsItem();

    [Fact] protected override Task GetByIdAsync_ShouldThrowException_WhenSelectorReturnsNull() => base.GetByIdAsync_ShouldThrowException_WhenSelectorReturnsNull();

    public BaseDataAccessFacadeTests(DepartmentFactory dataFactory) : base(dataFactory)
    {
    }
}