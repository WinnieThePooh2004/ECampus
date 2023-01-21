using ECampus.Infrastructure;
using ECampus.Infrastructure.DataDeleteServices;
using ECampus.Shared.Models;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.DataDelete;

public class DataDeleteTests 
{
    private readonly DataDeleteService<Auditory> _sut;

    public DataDeleteTests()
    {
        _sut = new DataDeleteService<Auditory>();
    }

    [Fact]
    public async Task Delete_ModelRemovedFromContext()
    {
        Auditory? deleted = null;
        var context = Substitute.For<ApplicationDbContext>();
        context.Remove(Arg.Do<Auditory>(model => deleted = model));

        await _sut.DeleteAsync(10, context);

        context.Received(1).Remove(Arg.Any<Auditory>());
        deleted?.Id.Should().Be(10);
    }
}