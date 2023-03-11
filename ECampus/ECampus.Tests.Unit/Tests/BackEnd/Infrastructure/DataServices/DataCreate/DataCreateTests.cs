using ECampus.DataAccess.DataCreateServices;
using ECampus.Domain.Entities;
using ECampus.Infrastructure;

namespace ECampus.Tests.Unit.Tests.BackEnd.Infrastructure.DataServices.DataCreate;

public class DataCreateTests
{
    private readonly ApplicationDbContext _context;
    private readonly DataCreateService<Auditory> _sut;

    public DataCreateTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _sut = new DataCreateService<Auditory>();
    }

    [Fact]
    protected void Create_AddedToContext()
    {
        var model = new Auditory();

        _sut.Create(model, _context);

        _context.Received(1).Add(model);
    }
}