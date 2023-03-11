using ECampus.DataAccess.DataUpdateServices;
using ECampus.DataAccess.Interfaces;
using ECampus.DataAccess.Relationships;
using ECampus.Domain.Entities;
using ECampus.Domain.Entities.RelationEntities;
using ECampus.Infrastructure;
using ECampus.Tests.Unit.InMemoryDb;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.Tests.BackEnd.Infrastructure.DataServices.DataUpdate;

public class ManyToManyRelationshipsUpdateTests
{
    private readonly ManyToManyRelationshipsUpdate<User, Auditory, UserAuditory> _sut;
    private readonly IDataUpdateService<User> _baseUpdate;

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
        await SeedData(100);
        var context = await InMemoryDbFactory.GetContext();
        var updatedEntity = new User
        {
            Id = 100,
            SavedAuditories = new List<Auditory>
            {
                new() { Id = 100 },
                new() { Id = 102 }
            }
        };

        await _sut.UpdateAsync(updatedEntity, context);
        await context.SaveChangesAsync();
        var currentRelations = await context.UserAuditories
            .Where(u => u.UserId == 100)
            .Select(u => new UserAuditory { UserId = u.UserId, AuditoryId = u.AuditoryId })
            .ToListAsync();

        await _baseUpdate.Received().UpdateAsync(updatedEntity, context);
        currentRelations.Should().BeEquivalentTo(new List<UserAuditory>
        {
            new() { UserId = 100, AuditoryId = 100 },
            new() { UserId = 100, AuditoryId = 102 }
        });
    }

    [Fact]
    public async Task Update_ShouldDeleteAllRelations_WhenNoRelatedModelsPassed()
    {
        await SeedData();
        var user = new User { Id = 1, SavedAuditories = new List<Auditory>() };
        await using var context = await InMemoryDbFactory.GetContext();

        await _sut.UpdateAsync(user, context);
        await context.SaveChangesAsync();

        (await context.UserAuditories.Where(u => u.UserId == 1).CountAsync()).Should().Be(0);
    }

    private static async Task SeedData(int firstItemId = 1)
    {
        await using var context = await InMemoryDbFactory.GetContext();
        context.Add(new User { Id = firstItemId, Username = firstItemId.ToString(), Email = firstItemId.ToString() });
        context.AddRange(Auditories(firstItemId));

        context.AddRange(UserAuditories(firstItemId));
        await context.SaveChangesAsync();
    }

    private static IEnumerable<Auditory> Auditories(int firstItemId = 1) => new List<Auditory>
    {
        new() { Id = firstItemId },
        new() { Id = firstItemId + 1 },
        new() { Id = firstItemId + 2 }
    };

    private static IEnumerable<UserAuditory> UserAuditories(int firstItemId = 1) => new List<UserAuditory>
    {
        new() { UserId = firstItemId, AuditoryId = firstItemId },
        new() { UserId = firstItemId, AuditoryId = firstItemId + 1 }
    };
}