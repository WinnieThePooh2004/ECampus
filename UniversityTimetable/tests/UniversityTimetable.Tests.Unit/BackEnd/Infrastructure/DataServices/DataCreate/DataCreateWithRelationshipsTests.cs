using Microsoft.EntityFrameworkCore;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataCreateServices;
using UniversityTimetable.Infrastructure.Relationships;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Interfaces.Data.DataServices;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.DataCreate;

public class DataCreateWithRelationshipsTests
{
    private readonly DataCreateWithRelationships<User, Auditory, UserAuditory> _sut;
    private readonly ApplicationDbContext _context;
    private readonly IDataCreateService<User> _baseCreateService;
    private readonly RelationshipsHandler<User, Auditory, UserAuditory> _relationshipsHandler = new();

    public DataCreateWithRelationshipsTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _baseCreateService = Substitute.For<IDataCreateService<User>>();
        _sut = new DataCreateWithRelationships<User, Auditory, UserAuditory>(_baseCreateService, _relationshipsHandler);
    }

    [Fact]
    public async Task CreateRelationModels_ShouldAddToModel()
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
        IEnumerable<object>? addedObjects = null;
        _context.AddRange(Arg.Do<IEnumerable<object>>(objects => addedObjects = objects));

        await _sut.CreateAsync(model, _context);

        addedObjects.Should().BeEquivalentTo(new List<UserAuditory>
        {
            new() { UserId = 3, AuditoryId = 1 },
            new() { UserId = 3, AuditoryId = 2 }
        }, opt => opt.ComparingByMembers<UserAuditory>());
        await _baseCreateService.Received().CreateAsync(model, _context);
    }

    [Fact]
    public async Task CreateRelationModels_ShouldThrowException_WhenRelatedModelsIsnull()
    {
        var model = new User();

        await new Func<Task>(() => _sut.CreateAsync(model, _context)).Should().ThrowAsync<InfrastructureExceptions>()
            .WithMessage(
                $"Please, send related models of object of type '{typeof(User)}' as empty list instead of null" +
                $"\nError code: 400");

        await _baseCreateService.DidNotReceive().CreateAsync(Arg.Any<User>(), Arg.Any<DbContext>());
    }
}