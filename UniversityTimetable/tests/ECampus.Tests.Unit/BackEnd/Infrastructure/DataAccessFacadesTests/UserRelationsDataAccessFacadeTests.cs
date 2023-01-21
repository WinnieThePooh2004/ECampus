using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Shared.Interfaces.Data.DataServices;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataAccessFacadesTests;

public class UserRelationsDataAccessFacadeTests
{
    private readonly UserRelationshipsDataAccessFacade _sut;

    private readonly IRelationsDataAccess<User, Auditory, UserAuditory> _userAuditoryRelationships =
        Substitute.For<IRelationsDataAccess<User, Auditory, UserAuditory>>();

    private readonly IRelationsDataAccess<User, Group, UserGroup> _userGroupRelationships =
        Substitute.For<IRelationsDataAccess<User, Group, UserGroup>>();

    private readonly IRelationsDataAccess<User, Teacher, UserTeacher> _userTeacherRelationships =
        Substitute.For<IRelationsDataAccess<User, Teacher, UserTeacher>>();

    private readonly DbContext _context = Substitute.For<DbContext>();

    public UserRelationsDataAccessFacadeTests()
    {
        _sut = new UserRelationshipsDataAccessFacade(_context, _userAuditoryRelationships, _userGroupRelationships, _userTeacherRelationships);
    }

    [Fact]
    public async Task SaveAuditory_ShouldCallRelationsDataAccess()
    {
        await _sut.SaveAuditory(10, 10);

        await _userAuditoryRelationships.Received(1).CreateRelation(10, 10, _context);
    }

    [Fact]
    public async Task SaveGroup_ShouldCallRelationsDataAccess()
    {
        await _sut.SaveGroup(10, 10);

        await _userGroupRelationships.Received(1).CreateRelation(10, 10, _context);
    }

    [Fact]
    public async Task SaveTeacher_ShouldCallRelationsDataAccess()
    {
        await _sut.SaveTeacher(10, 10);

        await _userTeacherRelationships.Received(1).CreateRelation(10, 10, _context);
    }

    [Fact]
    public async Task RemoveSavedAuditory_ShouldCallRelationsDataAccess()
    {
        await _sut.RemoveSavedAuditory(10, 10);

        await _userAuditoryRelationships.Received(1).DeleteRelation(10, 10, _context);
    }

    [Fact]
    public async Task RemoveSavedGroup_ShouldCallRelationsDataAccess()
    {
        await _sut.RemoveSavedGroup(10, 10);

        await _userGroupRelationships.Received(1).DeleteRelation(10, 10, _context);
    }

    [Fact]
    public async Task RemoveSavedTeacher_ShouldCallRelationsDataAccess()
    {
        await _sut.RemoveSavedTeacher(10, 10);

        await _userTeacherRelationships.Received(1).DeleteRelation(10, 10, _context);
    }
}