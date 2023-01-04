using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Tests.Shared.Mocks;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleDepartmentSelectorTests
{
    private readonly MultipleDepartmentSelector _sut;
    private readonly DbSet<Department> _dataSet;
    private readonly List<Department> _data;

    public MultipleDepartmentSelectorTests()
    {
        _sut = new MultipleDepartmentSelector();
        _data = new List<Department>
        {
            new() { Name = "b", FacultyId = 10 },
            new() { Name = "a", FacultyId = 10 },
            new() { Name = "a", FacultyId = 11 }
        };
        _dataSet = new DbSetMock<Department>(_data);
    }

    [Fact]
    public void SelectData_ShouldReturnSuitableData()
    {
        var parameters = new DepartmentParameters { DepartmentName = "a", FacultyId = 10 };

        var selectedData = _sut.SelectData(_dataSet, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Count().Should().Be(1);
    }
    
    [Fact]
    public void SelectData_ShouldNotFilterByFacultyId_WhenFacultyIdIs0()
    {
        var parameters = new DepartmentParameters { DepartmentName = "a", FacultyId = 0 };

        var selectedData = _sut.SelectData(_dataSet, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Should().Contain(_data[2]);
        selectedData.Count.Should().Be(2);
    }
}