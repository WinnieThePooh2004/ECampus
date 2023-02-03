using ECampus.Contracts.DataAccess;
using ECampus.Domain.Interfaces.Auth;
using ECampus.Services.Services;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public class UserRelationsServiceTests
{
    private readonly UserRelationsService _sut;
    private readonly IRelationsDataAccess _relationsDataAccess = Substitute.For<IRelationsDataAccess>();
    private readonly IAuthenticationService _authenticationService = Substitute.For<IAuthenticationService>();

    public UserRelationsServiceTests()
    {
        _sut = new UserRelationsService(_authenticationService, _relationsDataAccess);
    }
    
    [Fact]
    private async Task SaveGroup_RelationsRepositoryCalled()
    {
        await _sut.SaveGroup(10, 10);
    
        await _relationsDataAccess.Received(1).CreateRelation<User, Group, UserGroup>(10, 10);
    }
    
    [Fact]
    private async Task SaveAuditory_RelationsRepositoryCalled()
    {
        await _sut.SaveAuditory(10, 10);
    
        await _relationsDataAccess.Received(1).CreateRelation<User, Auditory, UserAuditory>(10, 10);
    }
    
    [Fact]
    private async Task SaveTeacher_RelationsRepositoryCalled()
    {
        await _sut.SaveTeacher(10, 10);
    
        await _relationsDataAccess.Received(1).CreateRelation<User, Teacher, UserTeacher>(10, 10);
    }
    
    [Fact]
    private async Task RemoveSavedGroup_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedGroup(10, 10);
    
        await _relationsDataAccess.Received(1).DeleteRelation<User, Group, UserGroup>(10, 10);
    }
    
    [Fact]
    private async Task RemoveSavedAuditory_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedAuditory(10, 10);
    
        await _relationsDataAccess.Received(1).DeleteRelation<User, Auditory, UserAuditory>(10, 10);
    }
    
    [Fact]
    private async Task RemoveSavedTeacher_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedTeacher(10, 10);
    
        await _relationsDataAccess.Received(1).DeleteRelation<User, Teacher, UserTeacher>(10, 10);
    }
}