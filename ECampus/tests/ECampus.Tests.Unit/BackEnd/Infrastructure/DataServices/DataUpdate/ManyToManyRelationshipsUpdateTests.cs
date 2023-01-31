using ECampus.Infrastructure;
using ECampus.Infrastructure.DataUpdateServices;
using ECampus.Infrastructure.Interfaces;
using ECampus.Infrastructure.Relationships;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;
using ECampus.Tests.Unit.InMemoryDb;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.DataUpdate;

public class ManyToManyRelationshipsUpdateTests
{
    private readonly ManyToManyRelationshipsUpdate<User, Auditory, UserAuditory> _sut;
    private readonly IDataUpdateService<User> _baseUpdate;
    private static bool _dataCreated;

    private readonly RelationshipsHandler<User, Auditory, UserAuditory> _handler = new();

    public ManyToManyRelationshipsUpdateTests()
    {
        _baseUpdate = Substitute.For<IDataUpdateService<User>>();
        _sut = new ManyToManyRelationshipsUpdate<User, Auditory, UserAuditory>(_baseUpdate, _handler);
    }

    [Fact]
    public async Task UpdateRelationModels_ShouldInstantlyReturnFromBaseCreate_WhenRelatedModelsIsnull()
    {
        var context = Substitute.For<ApplicationDbContext>();
        var model = new User();

        await _sut.UpdateAsync(model, context);

        context.DidNotReceive().Add(Arg.Any<object>());
        context.DidNotReceive().Remove(Arg.Any<UserAuditory>());
        await _baseUpdate.Received().UpdateAsync(model, context);
    }

    [Fact]
    private async Task UpdateRelations_ShouldUpdateRelations()
    {
        await SeedData();
        var context = await InMemoryDbFactory.GetContext();
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

        await _sut.UpdateAsync(updatedEntity, context);
        await context.SaveChangesAsync();
        var currentRelations = await context.UserAuditories
            .Select(u => new UserAuditory { UserId = u.UserId, AuditoryId = u.AuditoryId })
            .ToListAsync();

        await _baseUpdate.Received().UpdateAsync(updatedEntity, context);
        currentRelations.Should().BeEquivalentTo(new List<UserAuditory>
        {
            new() { UserId = 10, AuditoryId = 3 },
            new() { UserId = 10, AuditoryId = 4 },
            new() { UserId = 10, AuditoryId = 6 }
        });
    }

    [Fact]
    public async Task Update_ShouldDeleteAllRelations_WhenNoRelatedModelsPassed()
    {
        await SeedData();
        var user = new User { Id = 10, SavedAuditories = new List<Auditory>() };
        await using var context = await InMemoryDbFactory.GetContext();

        await _sut.UpdateAsync(user, context);
        await context.SaveChangesAsync();

        (await context.UserAuditories.CountAsync()).Should().Be(0);
    }

    private static async Task SeedData()
    {
        await using var context = await InMemoryDbFactory.GetContext();
        if (!_dataCreated)
        {
            context.Add(new User { Id = 10 });
            context.AddRange(Auditories);
            _dataCreated = true;
        }

        context.AddRange(UserAuditories);
        await context.SaveChangesAsync();
    }

    private static IEnumerable<Auditory> Auditories => new List<Auditory>
    {
        new() { Id = 3 },
        new() { Id = 4 },
        new() { Id = 5 },
        new() { Id = 6 }
    };

    private static IEnumerable<UserAuditory> UserAuditories => new List<UserAuditory>
    {
        new() { UserId = 10, AuditoryId = 3 },
        new() { UserId = 10, AuditoryId = 5 }
    };
}