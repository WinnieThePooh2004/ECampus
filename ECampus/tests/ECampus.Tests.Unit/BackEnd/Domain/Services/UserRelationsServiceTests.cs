using ECampus.Services.Services;
using ECampus.Shared.Interfaces.Auth;
using ECampus.Shared.Interfaces.DataAccess;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public class UserRelationsServiceTests
{
    private readonly UserRelationsService _sut;

    private readonly IUserRelationsDataAccessFacade _userDataAccessFacade =
        Substitute.For<IUserRelationsDataAccessFacade>();

    private readonly IAuthenticationService _authenticationService = Substitute.For<IAuthenticationService>();

    public UserRelationsServiceTests()
    {
        _sut = new UserRelationsService(_authenticationService, _userDataAccessFacade);
    }
    
    [Fact]
    private async Task SaveGroup_RelationsRepositoryCalled()
    {
        await _sut.SaveGroup(10, 10);
    
        await _userDataAccessFacade.Received(1).SaveGroup(10, 10);
    }
    
    [Fact]
    private async Task SaveAuditory_RelationsRepositoryCalled()
    {
        await _sut.SaveAuditory(10, 10);
    
        await _userDataAccessFacade.Received(1).SaveAuditory(10, 10);
    }
    
    [Fact]
    private async Task SaveTeacher_RelationsRepositoryCalled()
    {
        await _sut.SaveTeacher(10, 10);
    
        await _userDataAccessFacade.Received(1).SaveTeacher(10, 10);
    }
    
    [Fact]
    private async Task RemoveSavedGroup_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedGroup(10, 10);
    
        await _userDataAccessFacade.Received(1).RemoveSavedGroup(10, 10);
    }
    
    [Fact]
    private async Task RemoveSavedAuditory_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedAuditory(10, 10);
    
        await _userDataAccessFacade.Received(1).RemoveSavedAuditory(10, 10);
    }
    
    [Fact]
    private async Task RemoveSavedTeacher_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedTeacher(10, 10);
    
        await _userDataAccessFacade.Received(1).RemoveSavedTeacher(10, 10);
    }
}