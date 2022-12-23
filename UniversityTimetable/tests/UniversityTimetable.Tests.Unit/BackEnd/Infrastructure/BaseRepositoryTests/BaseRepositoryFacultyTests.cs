using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared.DataFactories;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.BaseRepositoryTests;

public class BaseRepositoryFacultyTests : BaseRepositoryTests<Faculty>, IClassFixture<FacultyFactory>
{
    [Fact] protected override Task Create_AddedToDb() => base.Create_AddedToDb();

    [Fact] protected override Task Create_ShouldThrowException_WhenModelIdNot0() => base.Create_ShouldThrowException_WhenModelIdNot0();

    [Fact] protected override Task Update_UpdatedInDb_IfExistsInDb() => base.Update_UpdatedInDb_IfExistsInDb();

    [Fact] protected override Task Update_ShouldThrowException_IfSaveChangeThrowsException() => base.Update_ShouldThrowException_IfSaveChangeThrowsException();

    [Fact] protected override Task GetById_ReturnsFromSelector_IfSelectorReturnsItem() => base.GetById_ReturnsFromSelector_IfSelectorReturnsItem();

    [Fact] protected override Task GetByIdAsync_ShouldThrowException_WhenSelectorReturnsNull() => base.GetByIdAsync_ShouldThrowException_WhenSelectorReturnsNull();

    public BaseRepositoryFacultyTests(FacultyFactory dataFactory) : base(dataFactory)
    {
        
    }
}