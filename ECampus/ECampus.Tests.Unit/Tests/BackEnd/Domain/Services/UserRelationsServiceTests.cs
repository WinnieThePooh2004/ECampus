using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.Domain.Entities;
using ECampus.Domain.Entities.RelationEntities;
using ECampus.Services.Contracts.Services;
using ECampus.Services.Services;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Services;

public class UserRelationsServiceTests
{
    private readonly UserRelationsService _sut;
    private readonly IRelationsDataAccess _relationsDataAccess = Substitute.For<IRelationsDataAccess>();
    private readonly IAuthenticationService _authenticationService = Substitute.For<IAuthenticationService>();

    public UserRelationsServiceTests()
    {
        _sut = new UserRelationsService(_authenticationService, _relationsDataAccess,
            Substitute.For<IDataAccessFacade>());
    }

    [Fact]
    private async Task SaveGroup_RelationsRepositoryCalled()
    {
        await _sut.SaveGroup(10, 10);

        _relationsDataAccess.Received(1).CreateRelation<User, Group, UserGroup>(10, 10);
    }

    [Fact]
    private async Task SaveAuditory_RelationsRepositoryCalled()
    {
        await _sut.SaveAuditory(10, 10);

        _relationsDataAccess.Received(1).CreateRelation<User, Auditory, UserAuditory>(10, 10);
    }

    [Fact]
    private async Task SaveTeacher_RelationsRepositoryCalled()
    {
        await _sut.SaveTeacher(10, 10);

        _relationsDataAccess.Received(1).CreateRelation<User, Teacher, UserTeacher>(10, 10);
    }

    [Fact]
    private async Task RemoveSavedGroup_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedGroup(10, 10);

        _relationsDataAccess.Received(1).DeleteRelation<User, Group, UserGroup>(10, 10);
    }

    [Fact]
    private async Task RemoveSavedAuditory_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedAuditory(10, 10);

        _relationsDataAccess.Received(1).DeleteRelation<User, Auditory, UserAuditory>(10, 10);
    }

    [Fact]
    private async Task RemoveSavedTeacher_RelationsRepositoryCalled()
    {
        await _sut.RemoveSavedTeacher(10, 10);

        _relationsDataAccess.Received(1).DeleteRelation<User, Teacher, UserTeacher>(10, 10);
    }
}