using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataCreateServices;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.DataCreate;

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
    protected async Task Create_AddedToContext()
    {
        var model = new Auditory();

        await _sut.CreateAsync(model, _context);

        await _context.Received(1).AddAsync(model);
    }
}