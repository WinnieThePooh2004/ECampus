using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Tests.Shared.Mocks.EntityFramework;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleStudentSelectorTests
{
    private readonly MultipleStudentSelector _sut;
    private readonly DbSet<Student> _dataSet;
    private readonly List<Student> _data;

    public MultipleStudentSelectorTests()
    {
        _sut = new MultipleStudentSelector();
        _data = new List<Student>
        {
            new() { LastName = "b", GroupId = 10 },
            new() { LastName = "ab", GroupId = 10 },
            new() { LastName = "ab", GroupId = 11 }
        };
        _dataSet = new DbSetMock<Student>(_data);
    }

    [Fact]
    public void SelectData_ShouldReturnSuitableData()
    {
        var parameters = new StudentParameters { GroupId = 10, LastName = "a", FirstName = ""};

        var selectedData = _sut.SelectData(_dataSet, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Count.Should().Be(1);
    }
    
    [Fact]
    public void SelectData_ShouldNotFilterByFacultyId_WhenFacultyIdIs0()
    {
        var parameters = new StudentParameters { GroupId = 0, LastName = "a", FirstName = "" };

        var selectedData = _sut.SelectData(_dataSet, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Should().Contain(_data[2]);
        selectedData.Count.Should().Be(2);
    }
}