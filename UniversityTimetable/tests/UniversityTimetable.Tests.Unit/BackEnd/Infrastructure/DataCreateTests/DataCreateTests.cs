using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataCreate;
using UniversityTimetable.Shared.Interfaces.Data;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataCreateTests;

public class DataCreateTests<TModel>
    where TModel : class, IModel, new()
{
    private readonly ApplicationDbContext _context;
    private readonly DataCreate<TModel> _sut;

    protected DataCreateTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _sut = new DataCreate<TModel>();
    }

    protected virtual async Task Create_AddedToContext()
    {
        var model = new TModel();

        await _sut.CreateAsync(model, _context);

        await _context.Received(1).AddAsync(model);
    }
}