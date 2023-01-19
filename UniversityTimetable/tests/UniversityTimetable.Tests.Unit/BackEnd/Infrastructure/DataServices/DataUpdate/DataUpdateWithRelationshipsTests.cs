using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataUpdateServices;
using UniversityTimetable.Infrastructure.Relationships;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;
using UniversityTimetable.Tests.Shared.Mocks.EntityFramework;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.DataUpdate;

public class DataUpdateWithRelationshipsTests
{
    private readonly DataUpdateServiceWithRelationships<User, Auditory, UserAuditory> _sut;
    private readonly ApplicationDbContext _context;
    private readonly IDataUpdateService<User> _baseUpdate;

    private readonly RelationshipsHandler<User, Auditory, UserAuditory> _handler = new();

    public DataUpdateWithRelationshipsTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _baseUpdate = Substitute.For<IDataUpdateService<User>>();
        _sut = new DataUpdateServiceWithRelationships<User, Auditory, UserAuditory>(_baseUpdate, _handler);
    }

    [Fact]
    public async Task UpdateRelationModels_ShouldThrowException_WhenRelatedModelsIsnull()
    {
        var model = new User();
        var set = new DbSetMock<UserAuditory>().Object;
        _context.Set<UserAuditory>().Returns(set);

        await new Func<Task>(() => _sut.UpdateAsync(model, _context)).Should()
            .ThrowAsync<RelatedModelsIsNullException>()
            .WithMessage(new RelatedModelsIsNullException(model, typeof(User)).Message);
    }

    [Fact]
    public async Task UpdateRelations_ShouldUpdateRelations()
    {
        var dataSource = new List<UserAuditory>
        {
            new() { UserId = 10, AuditoryId = 3 },
            new() { UserId = 10, AuditoryId = 5 }
        };
        var auditories = new List<Auditory>
        {
            new() { Id = 3, UsersIds = new List<UserAuditory> { new() { UserId = 10, AuditoryId = 3 } } },
            new() { Id = 4, UsersIds = new List<UserAuditory>() },
            new() { Id = 5, UsersIds = new List<UserAuditory> { new() { UserId = 10, AuditoryId = 5 } } },
            new() { Id = 6, UsersIds = new List<UserAuditory>() },
        };
        List<UserAuditory>? deleted = null;
        List<UserAuditory>? added = null;
        var currentRelationModels = new DbSetMock<UserAuditory>(dataSource).Object;
        var auditoriesSet = new DbSetMock<Auditory>(auditories).Object;
        _context.Set<UserAuditory>().Returns(currentRelationModels);
        _context.Set<Auditory>().Returns(auditoriesSet);
        _context.RemoveRange(Arg.Do<IEnumerable<object>>(list => deleted = list
            .Select(i => (UserAuditory)i).ToList()));
        _context.AddRange(Arg.Do<IEnumerable<object>>(list => added = list
            .Select(i => (UserAuditory)i).ToList()));
        var updatedEntity = new User
        {
            Id = 10,
            SavedAuditories = new List<Auditory>
            {
                new() { Id = 3 },
                new() { Id = 4 },
                new() { Id = 6 }
            }
        };

        await _sut.UpdateAsync(updatedEntity, _context);

        deleted.Should().BeEquivalentTo(new List<UserAuditory>
        {
            new() { UserId = 10, AuditoryId = 5 }
        });

        added.Should().BeEquivalentTo(new List<UserAuditory>
        {
            new() { UserId = 10, AuditoryId = 4 },
            new() { UserId = 10, AuditoryId = 6 }
        });

        await _baseUpdate.Received().UpdateAsync(updatedEntity, _context);
    }
}