using ECampus.Infrastructure;
using ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleFacultySelectorTests
{
    private readonly MultipleFacultySelector _sut;
    private readonly List<Faculty> _data;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public MultipleFacultySelectorTests()
    {
        _sut = new MultipleFacultySelector();
        _data = new List<Faculty>
        {
            new() { Name = "name1" },
            new() { Name = "name2" },
            new() { Name = "nam" }
        };
        _context.Faculties = new DbSetMock<Faculty>(_data);
    }

    [Fact]
    public void SelectData_ShouldReturnSuitableData()
    {
        var parameters = new FacultyParameters { Name = "name" };

        var selectedData = _sut.SelectData(_context, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Should().Contain(_data[0]);
        selectedData.Count.Should().Be(2);
    }
}