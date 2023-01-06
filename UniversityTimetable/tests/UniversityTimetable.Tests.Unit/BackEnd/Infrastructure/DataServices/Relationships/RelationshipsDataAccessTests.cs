using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.Relationships;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;
using UniversityTimetable.Tests.Shared.Mocks.EntityFramework;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.Relationships;

public class RelationshipsDataAccessTests
{
    private readonly RelationshipsDataAccess<User, Auditory, UserAuditory> _sut;
    private readonly ApplicationDbContext _context;

    public RelationshipsDataAccessTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _sut = new RelationshipsDataAccess<User, Auditory, UserAuditory>();
    }
    
    [Fact]
    public void CreateRelationModels_ShouldAddToModel()
    {
        var model = new User
        {
            Id = 3,
            SavedAuditories = new List<Auditory>
            {
                new() { Id = 1 },
                new() { Id = 2 }
            }
        };

        _sut.CreateRelationModels(model);

        model.SavedAuditoriesIds.Should().BeEquivalentTo(new List<UserAuditory>
        {
            new() { UserId = 3, AuditoryId = 1 },
            new() { UserId = 3, AuditoryId = 2 }
        }, opt => opt.ComparingByMembers<UserAuditory>());
    }
    
    [Fact]
    public void CreateRelationModels_ShouldThrowException_WhenRelatedModelsIsnull()
    {
        var model = new User();

        new Action(() => _sut.CreateRelationModels(model)).Should().Throw<InfrastructureExceptions>()
            .WithMessage($"Please, send related models of object of type '{typeof(User)}' as empty list instead of null" +
                         $"\nError code: 400");
    }
    
    [Fact]
    public void UpdateRelationModels_ShouldThrowException_WhenRelatedModelsIsnull()
    {
        var model = new User();

        new Action(() => _sut.CreateRelationModels(model)).Should().Throw<InfrastructureExceptions>()
            .WithMessage($"Please, send related models of object of type '{typeof(User)}' as empty list instead of null" +
                         $"\nError code: 400");
    }

    [Fact]
    public async Task UpdateRelations_ShouldUpdateRelations()
    {
        var dataSource = new List<UserAuditory>
        {
            new() { UserId = 10, AuditoryId = 3 },
            new() { UserId = 10, AuditoryId = 5 },
        };
        List<UserAuditory>? deleted = null;
        List<UserAuditory>? added = null;
        var currentRelationModels = new DbSetMock<UserAuditory>(dataSource).Object;
        _context.Set<UserAuditory>().Returns(currentRelationModels);
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
                new() { Id = 4 }
            }
        };

        await _sut.UpdateRelations(updatedEntity, _context);

        deleted.Should().BeEquivalentTo(new List<UserAuditory>
        {
            new() { UserId = 10, AuditoryId = 5 }
        });

        added.Should().BeEquivalentTo(new List<UserAuditory>
        {
            new() { UserId = 10, AuditoryId = 4 }
        });
    }
}