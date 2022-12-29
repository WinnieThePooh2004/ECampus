using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataDelete;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataDeleteTests;

public class DataDeleteTests<TModel> 
    where TModel : class, IModel, new()
{
    private readonly DataDelete<TModel> _sut;

    protected DataDeleteTests()
    {
        _sut = new DataDelete<TModel>();
    }

    protected virtual async Task Delete_ModelRemovedFromContext()
    {
        TModel? deleted = null;
        var context = Substitute.For<ApplicationDbContext>();
        context.Remove(Arg.Do<TModel>(model => deleted = model));

        await _sut.DeleteAsync(10, context);

        context.Received(1).Remove(Arg.Any<TModel>());
        deleted?.Id.Should().Be(10);
    }
}