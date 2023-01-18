using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Infrastructure.DataAccessFacades;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataAccessFacadesTests;

public class UserRelationsDataAccessFacadeTests
{
    private readonly UserRelationshipsDataAccessFacade _sut;
    private readonly IRelationsDataAccess _relationsDataAccess = Substitute.For<IRelationsDataAccess>();
    private readonly DbContext _context = Substitute.For<DbContext>();

    public UserRelationsDataAccessFacadeTests()
    {
        _sut = new UserRelationshipsDataAccessFacade(_relationsDataAccess, _context);
    }
    
    [Fact]
    public async Task SaveAuditory_ShouldCallRelationsDataAccess()
    {
        await _sut.SaveAuditory(10, 10);

        await _relationsDataAccess.Received(1).CreateRelation<UserAuditory, User, Auditory>(10, 10, _context);
    }

    [Fact]
    public async Task SaveGroup_ShouldCallRelationsDataAccess()
    {
        await _sut.SaveGroup(10, 10);

        await _relationsDataAccess.Received(1).CreateRelation<UserGroup, User, Group>(10, 10, _context);
    }

    [Fact]
    public async Task SaveTeacher_ShouldCallRelationsDataAccess()
    {
        await _sut.SaveTeacher(10, 10);

        await _relationsDataAccess.Received(1).CreateRelation<UserTeacher, User, Teacher>(10, 10, _context);
    }

    [Fact]
    public async Task RemoveSavedAuditory_ShouldCallRelationsDataAccess()
    {
        await _sut.RemoveSavedAuditory(10, 10);

        await _relationsDataAccess.Received(1).DeleteRelation<UserAuditory, User, Auditory>(10, 10, _context);
    }

    [Fact]
    public async Task RemoveSavedGroup_ShouldCallRelationsDataAccess()
    {
        await _sut.RemoveSavedGroup(10, 10);

        await _relationsDataAccess.Received(1).DeleteRelation<UserGroup, User, Group>(10, 10, _context);
    }

    [Fact]
    public async Task RemoveSavedTeacher_ShouldCallRelationsDataAccess()
    {
        await _sut.RemoveSavedTeacher(10, 10);

        await _relationsDataAccess.Received(1).DeleteRelation<UserTeacher, User, Teacher>(10, 10, _context);
    }
}