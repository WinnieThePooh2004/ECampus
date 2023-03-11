using ECampus.DataAccess.DataUpdateServices;
using ECampus.Infrastructure;
using ECampus.Shared.Models;

namespace ECampus.Tests.Unit.Tests.BackEnd.Infrastructure.DataServices.DataUpdate;

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