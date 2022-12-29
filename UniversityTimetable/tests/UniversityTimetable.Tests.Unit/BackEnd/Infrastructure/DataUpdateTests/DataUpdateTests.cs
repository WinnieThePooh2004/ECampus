using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataUpdate;
using UniversityTimetable.Shared.Interfaces.Data.Models;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataUpdateTests;

public class DataUpdateTests<TModel> where TModel : class, IModel, new()
{
    private readonly DataUpdate<TModel> _sut;

    protected DataUpdateTests()
    {
        _sut = new DataUpdate<TModel>();
    }

    protected virtual async Task Update_ModelRemovedFromContext()
    {
        var model = new TModel();
        var context = Substitute.For<ApplicationDbContext>();

        await _sut.UpdateAsync(model, context);

        context.Received(1).Update(model);
    }
}