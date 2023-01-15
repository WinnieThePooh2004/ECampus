using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Tests.Shared.Mocks.EntityFramework;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleFacultySelectorTests
{
    private readonly MultipleFacultySelector _sut;
    private readonly DbSet<Faculty> _dataSet;
    private readonly List<Faculty> _data;

    public MultipleFacultySelectorTests()
    {
        _sut = new MultipleFacultySelector();
        _data = new List<Faculty>
        {
            new() { Name = "name1" },
            new() { Name = "name2" },
            new() { Name = "nam" }
        };
        _dataSet = new DbSetMock<Faculty>(_data);
    }

    [Fact]
    public void SelectData_ShouldReturnSuitableData()
    {
        var parameters = new FacultyParameters { Name = "name" };

        var selectedData = _sut.SelectData(_dataSet, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Should().Contain(_data[0]);
        selectedData.Count.Should().Be(2);
    }
}