using ECampus.DataAccess.DataSelectors.MultipleItemSelectors;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using ECampus.Shared.QueryParameters;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.ParametersSelector;

public class MultipleSubjectSelectorTests
{
    private readonly MultipleSubjectSelector _sut;
    private readonly List<Subject> _data;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public MultipleSubjectSelectorTests()
    {
        _sut = new MultipleSubjectSelector();
        _data = new List<Subject>
        {
            new() { Name = "name1" },
            new() { Name = "name2" },
            new() { Name = "nam" }
        };
        _context.Subjects = new DbSetMock<Subject>(_data);
    }

    [Fact]
    public void SelectData_ShouldReturnSuitableData()
    {
        var parameters = new SubjectParameters { Name = "name" };

        var selectedData = _sut.SelectData(_context, parameters).ToList();

        selectedData.Should().Contain(_data[1]);
        selectedData.Should().Contain(_data[0]);
        selectedData.Count.Should().Be(2);
    }
}