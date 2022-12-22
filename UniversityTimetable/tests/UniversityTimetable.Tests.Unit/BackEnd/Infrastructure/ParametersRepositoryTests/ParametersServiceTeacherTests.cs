using UniversityTimetable.Infrastructure.DataSelectors;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Tests.Shared.DataFactories;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.ParametersRepositoryTests;

public class ParametersServiceTeacherTests : ParametersRepositoryTests<Teacher, TeacherParameters>,
    IClassFixture<TeacherFactory>,
    IClassFixture<TeacherSelector>
{
    public ParametersServiceTeacherTests(TeacherFactory factory, TeacherSelector selector) 
        : base(factory, selector)
    {
    }

    [Fact] protected override Task Delete_BaseRepositoryCalled() => base.Delete_BaseRepositoryCalled();

    [Fact] protected override Task Create_ReturnsFromBaseRepository_BaseRepositoryCalled() => base.Create_ReturnsFromBaseRepository_BaseRepositoryCalled();

    [Fact] protected override Task Update_ReturnsFromBaseRepository_BaseRepositoryCalled() => base.Update_ReturnsFromBaseRepository_BaseRepositoryCalled();

    [Fact] protected override Task GetById_ReturnsFromBaseRepository_BaseRepositoryCalled() => base.GetById_ReturnsFromBaseRepository_BaseRepositoryCalled();

    [Fact] protected Task GetByParameters_DepartmentId0_ReturnsAllFromDb() => GetByParameters_ReturnsFromDb();

    [Fact]
    protected async Task GetByParameters_DepartmentId1_ReturnsListWhereDepartmentId1()
    {
        var data = Enumerable.Range(0, 10).Select(i => CreateModel()).ToList();
        for (var i = 0; i < 3; ++i)
        {
            data[i].DepartmentId = 1;
        }
        var expected = data.Where(t => t.DepartmentId == 1).OrderBy(t => t.Id).ToList();

        await GetByParameters(new TeacherParameters { PageSize = 5, DepartmentId = 1 }, data, expected);
    }
}