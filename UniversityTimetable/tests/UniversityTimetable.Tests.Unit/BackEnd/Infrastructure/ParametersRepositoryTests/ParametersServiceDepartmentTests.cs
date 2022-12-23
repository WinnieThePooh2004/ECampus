using UniversityTimetable.Infrastructure.DataSelectors;
using UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Tests.Shared.DataFactories;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.ParametersRepositoryTests;

public class ParametersServiceDepartmentTests : ParametersRepositoryTests<Department, DepartmentParameters>,
    IClassFixture<DepartmentFactory>,
    IClassFixture<DepartmentSelector>
{
    public ParametersServiceDepartmentTests(DepartmentFactory factory, DepartmentSelector selector) 
        : base(factory, selector)
    {
    }

    [Fact] protected override Task Delete_BaseRepositoryCalled() => base.Delete_BaseRepositoryCalled();

    [Fact] protected override Task Create_ReturnsFromBaseRepository_BaseRepositoryCalled() => base.Create_ReturnsFromBaseRepository_BaseRepositoryCalled();

    [Fact] protected override Task Update_ReturnsFromBaseRepository_BaseRepositoryCalled() => base.Update_ReturnsFromBaseRepository_BaseRepositoryCalled();

    [Fact] protected override Task GetById_ReturnsFromBaseRepository_BaseRepositoryCalled() => base.GetById_ReturnsFromBaseRepository_BaseRepositoryCalled();

    [Fact] protected override Task GetByParameters_ReturnsFromDb() => base.GetByParameters_ReturnsFromDb();
    
}