using ECampus.Contracts.DataAccess;
using ECampus.DataAccess.Relationships;
using ECampus.Infrastructure;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.Relationships;

public class RelationsDataAccessTests
{
    private readonly ApplicationDbContext _context;
    private readonly RelationsDataAccess _sut;
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();
    private readonly RelationshipsHandler<User, Group, UserGroup> _relationshipsHandler = new();

    public RelationsDataAccessTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _sut = new RelationsDataAccess(_context, _serviceProvider);
        _serviceProvider.GetService(typeof(IRelationshipsHandler<User, Group, UserGroup>))
            .Returns(_relationshipsHandler);
    }

    [Fact]
    public void AddRelation_ShouldAddToDb_IfDbNotThrowExceptions()
    {
        _sut.CreateRelation<User, Group, UserGroup>(1, 2);

        _context.Received().Add(Arg.Is<UserGroup>(u => u.UserId == 1 && u.GroupId == 2));
    }

    [Fact]
    public void DeleteRelation_RemovedFromToDb_IfDbNotThrowExceptions()
    {
        UserGroup? deletedObject = null;
        _context.Remove(Arg.Do<UserGroup>(o => deletedObject = o));

        _sut.DeleteRelation<User, Group, UserGroup>(1, 2);

        deletedObject.Should().NotBeNull();
        deletedObject?.UserId.Should().Be(1);
        deletedObject?.GroupId.Should().Be(2);
    }
}