using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Infrastructure.DataSelectors.SingleItemSelectors;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Tests.Shared.DataFactories;
using UniversityTimetable.Tests.Shared.Extensions;
using UniversityTimetable.Tests.Shared.Mocks;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.SingleItemSelector;

public class SingleSubjectSelectorTests : IClassFixture<SubjectFactory>
{
    private readonly SingleSubjectSelector _selector;
    private readonly List<Subject> _data;
    private readonly DbSet<Subject> _set;

    public SingleSubjectSelectorTests(SubjectFactory dataFactory)
    {
        _data = dataFactory.CreateMany(new Fixture(), 10);
        _set = new DbSetMock<Subject>(_data).Object;
        _selector = new SingleSubjectSelector();
    }

    [Fact]
    public async Task SelectModel_ReturnsModel_IfModelExists_IncludeCalled()
    {
        var model = await _selector.SelectModel(_data[0].Id, _set);

        model.Should().Be(_data[0]);
        _set.Received().Include(t => t.Teachers);
    }

    [Fact]
    public async Task SelectModel_ReturnsNull_WhenObjectDoesNotExist()
    {
        var model = await _selector.SelectModel(-1, _set);
        model.Should().BeNull();
    }
}