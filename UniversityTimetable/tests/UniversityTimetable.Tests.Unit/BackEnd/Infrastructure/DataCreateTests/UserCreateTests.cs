using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataCreate;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Interfaces.DataAccess;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataCreateTests;

public class UserCreateTests
{
    private readonly UserCreate _sut;
    private readonly IRelationshipsDataAccess<User, Auditory, UserAuditory> _userAuditoryRelationships;
    private readonly IRelationshipsDataAccess<User, Group, UserGroup> _userGroupRelationships;
    private readonly IRelationshipsDataAccess<User, Teacher, UserTeacher> _userTeacherRelationships;
    private readonly IDataCreate<User> _baseService;

    public UserCreateTests()
    {
        _userAuditoryRelationships = Substitute.For<IRelationshipsDataAccess<User, Auditory, UserAuditory>>();
        _userGroupRelationships = Substitute.For<IRelationshipsDataAccess<User, Group, UserGroup>>();
        _userTeacherRelationships = Substitute.For<IRelationshipsDataAccess<User, Teacher, UserTeacher>>();
        _baseService = Substitute.For<IDataCreate<User>>();
        _sut = new UserCreate(_baseService, _userTeacherRelationships, _userGroupRelationships,
            _userAuditoryRelationships);
    }

    [Fact]
    public async Task Create_BaseServiceCalled_RelationshipsRepositoriesCalled()
    {
        var model = new User();
        var context = Substitute.For<ApplicationDbContext>();
        await _sut.CreateAsync(model, context);

        _userAuditoryRelationships.Received(1).CreateRelationModels(model);
        _userGroupRelationships.Received(1).CreateRelationModels(model);
        _userTeacherRelationships.Received(1).CreateRelationModels(model);
        await _baseService.Received(1).CreateAsync(model, context);
    }
}