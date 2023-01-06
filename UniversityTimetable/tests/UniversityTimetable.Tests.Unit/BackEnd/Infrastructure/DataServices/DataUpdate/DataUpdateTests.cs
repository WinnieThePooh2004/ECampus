using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataUpdateServices;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.DataUpdate;

public class DataUpdateTests
{
    private readonly DataUpdateService<Auditory> _sut;

    public DataUpdateTests()
    {
        _sut = new DataUpdateService<Auditory>();
    }

    [Fact]
    public async Task Update_ModelRemovedFromContext()
    {
        var model = new Auditory();
        var context = Substitute.For<ApplicationDbContext>();

        await _sut.UpdateAsync(model, context);

        context.Received(1).Update(model);
    }
}