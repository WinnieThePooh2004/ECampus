using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Infrastructure.DataSelectors.MultipleItemSelectors;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Tests.Shared.Mocks;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleAuditorySelectorTests
{
    private readonly MultipleAuditorySelector _sut;
    private readonly DbSet<Auditory> _dataSet;
    private readonly List<Auditory> _data;

    public MultipleAuditorySelectorTests()
    {
        _sut = new MultipleAuditorySelector();
        _data = new List<Auditory>
        {
            new(){ Name = "name1", Building = "build1"},
            new(){ Name = "name2", Building = "h" },
            new(){ Name = "nam", Building = "ht"}
        };
        _dataSet = new DbSetMock<Auditory>(_data);
    }

    [Fact]
    public void SelectData_ShouldReturnSuitableData()
    {
        var parameters = new AuditoryParameters { AuditoryName = "name", BuildingName = "h" };

        var selectedData = _sut.SelectData(_dataSet, parameters).ToList();
        
        selectedData.Should().Contain(_data[1]);
        selectedData.Count().Should().Be(1);
    }
}
