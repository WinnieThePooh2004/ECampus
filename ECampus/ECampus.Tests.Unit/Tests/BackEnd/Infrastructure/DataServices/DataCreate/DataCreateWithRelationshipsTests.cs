using ECampus.DataAccess.DataCreateServices;
using ECampus.DataAccess.Interfaces;
using ECampus.DataAccess.Relationships;
using ECampus.Domain.Models;
using ECampus.Domain.Models.RelationModels;
using ECampus.Infrastructure;

namespace ECampus.Tests.Unit.Tests.BackEnd.Infrastructure.DataServices.DataCreate;

public class DataCreateWithRelationshipsTests
{
    private readonly ManyToManyRelationshipsCreate<User, Auditory, UserAuditory> _sut;
    private readonly ApplicationDbContext _context;
    private readonly IDataCreateService<User> _baseCreateService;
    private readonly RelationshipsHandler<User, Auditory, UserAuditory> _relationshipsHandler = new();

    public DataCreateWithRelationshipsTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _baseCreateService = Substitute.For<IDataCreateService<User>>();
        _sut = new ManyToManyRelationshipsCreate<User, Auditory, UserAuditory>(_baseCreateService, _relationshipsHandler);
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

        _sut.Create(model, _context);

        model.SavedAuditoriesIds.Should().BeEquivalentTo(new List<UserAuditory>
        {
            new() { UserId = 3, AuditoryId = 1 },
            new() { UserId = 3, AuditoryId = 2 }
        }, opt => opt.ComparingByMembers<UserAuditory>());
        model.SavedAuditories.Should().BeNull();
        _baseCreateService.Received(1).Create(model, _context);
    }

    [Fact]
    public void CreateRelationModels_ShouldAddEmptyArray_WhenRelatedModelsIsnull()
    {
        var model = new User { SavedAuditories = null };

        _sut.Create(model, _context);

        model.SavedAuditoriesIds.Should().BeNull();
    }
}