using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataCreateServices;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.DataCreate;

public class DataCreateWithRelationshipsTests
{
    private readonly DataCreateServiceWithRelationships<User, Teacher, UserTeacher> _sut;
    private readonly IRelationshipsDataAccess<User, Teacher, UserTeacher> _relationships;
    private readonly ApplicationDbContext _context;
    private readonly IDataCreateService<User> _baseCreateService;

    public DataCreateWithRelationshipsTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _relationships = Substitute.For<IRelationshipsDataAccess<User, Teacher, UserTeacher>>();
        _baseCreateService = Substitute.For<IDataCreateService<User>>();
        _sut = new DataCreateServiceWithRelationships<User, Teacher, UserTeacher>(_relationships, _baseCreateService);
    }

    [Fact]
    public async Task Create_ShouldCallRelationshipsAndBaseCreate()
    {
        var model = new User();

        await _sut.CreateAsync(model, _context);

        _relationships.Received(1).CreateRelationModels(model);
        await _baseCreateService.Received(1).CreateAsync(model, _context);
    }
}