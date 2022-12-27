using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataCreateTests;

public class DataCreateDepartmentTests : DataCreateTests<Department>
{
    [Fact] protected override Task Create_AddedToContext() => base.Create_AddedToContext();
}