using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataUpdateServices;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.DataUpdate;

public class DataUpdateWithRelationshipsTests
{
    private readonly DataUpdateServiceWithRelationships<User, Teacher, UserTeacher> _sut;
    private readonly IRelationshipsDataAccess<User, Teacher, UserTeacher> _relationships;
    private readonly ApplicationDbContext _context;
    private readonly IDataUpdateService<User> _baseCreate;

    public DataUpdateWithRelationshipsTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _relationships = Substitute.For<IRelationshipsDataAccess<User, Teacher, UserTeacher>>();
        _baseCreate = Substitute.For<IDataUpdateService<User>>();
        _sut = new DataUpdateServiceWithRelationships<User, Teacher, UserTeacher>(_relationships, _baseCreate);
    }

    [Fact]
    public async Task Create_ShouldCallRelationshipsAndBaseCreate()
    {
        var model = new User();

        await _sut.UpdateAsync(model, _context);

        await _relationships.Received(1).UpdateRelations(model, _context);
        await _baseCreate.Received(1).UpdateAsync(model, _context);
    }
}