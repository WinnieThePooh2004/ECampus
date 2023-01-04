using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataAccessFacades;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataAccessFacadesTests;

public class UserDataAccessFacadeTests
{
    private readonly UserDataAccessFacade _sut;
    private readonly ApplicationDbContext _context;
    private readonly IPasswordChange _passwordChange;
    private readonly IRelationsDataAccess _relationsDataAccess;

    public UserDataAccessFacadeTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _passwordChange = Substitute.For<IPasswordChange>();
        _relationsDataAccess = Substitute.For<IRelationsDataAccess>();
        _sut = new UserDataAccessFacade(_context, _passwordChange, _relationsDataAccess);
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

    [Fact]
    public async Task ChangePassword_ShouldClassPasswordChange()
    {
        var passwordChange = new PasswordChangeDto();

        await _sut.ChangePassword(passwordChange);

        await _passwordChange.Received(1).ChangePassword(passwordChange, _context);
    }
}