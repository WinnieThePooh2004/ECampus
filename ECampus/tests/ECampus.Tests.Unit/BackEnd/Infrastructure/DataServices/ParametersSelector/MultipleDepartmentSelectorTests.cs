using ECampus.Infrastructure;
using ECampus.Infrastructure.DataSelectors.MultipleItemSelectors;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleDepartmentSelectorTests
{
    private readonly MultipleDepartmentSelector _sut;
    private readonly List<Department> _data;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public MultipleDepartmentSelectorTests()
    {
        _sut = new MultipleDepartmentSelector();
        _data = new List<Department>
        {
            new() { Name = "b", FacultyId = 10 },
            new() { Name = "a", FacultyId = 10 },
            new() { Name = "a", FacultyId = 11 }
        };
        _context.Departments = new DbSetMock<Department>(_data);
    }

    [Fact]
    public void SelectData_ShouldReturnSuitableData()
    {
        var parameters = new DepartmentParameters { DepartmentName = "a", FacultyId = 10 };

        var selectedData = _sut.SelectData(_context, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Count().Should().Be(1);
    }

    [Fact]
    public void SelectData_ShouldNotFilterByFacultyId_WhenFacultyIdIs0()
    {
        var parameters = new DepartmentParameters { DepartmentName = "a", FacultyId = 0 };

        var selectedData = _sut.SelectData(_context, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Should().Contain(_data[2]);
        selectedData.Count.Should().Be(2);
    }
}