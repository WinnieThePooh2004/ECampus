using ECampus.Infrastructure;
using ECampus.Infrastructure.Interfaces;
using ECampus.Infrastructure.Relationships;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Shared.Models.RelationModels;
using Microsoft.EntityFrameworkCore;

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
    public async Task AddRelation_ShouldAddToDb_IfDbNotThrowExceptions()
    {
        await _sut.CreateRelation<User, Group, UserGroup>(1, 2);

        _context.Received().Add(Arg.Is<UserGroup>(u => u.UserId == 1 && u.GroupId == 2));
    }

    [Fact]
    public async Task AddRelation_ShouldThrowException_IfErrorOccuredWhileSaveChanges()
    {
        _context.SaveChangesAsync().Returns(0).AndDoes(_ => throw new DbUpdateConcurrencyException("Some message"));

        await new Func<Task>(() => _sut.CreateRelation<User, Group, UserGroup>(0, 0)).Should()
            .ThrowAsync<InfrastructureExceptions>()
            .WithMessage($"cannot add relation between object of type {typeof(User)} with id=0 " +
                         $"on between object of type {typeof(Group)} with id=0\nError code: 404");
    }

    [Fact]
    public async Task AddRelation_ShouldThrowUnhandledException_IfExceptionWasThrown()
    {
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new DbUpdateException("DbUpdate message"));

        await new Func<Task>(() => _sut.CreateRelation<User, Group, UserGroup>(0, 0)).Should()
            .ThrowAsync<UnhandledInfrastructureException>()
            .WithMessage(new UnhandledInfrastructureException(new Exception()).Message)
            .WithInnerException<UnhandledInfrastructureException, DbUpdateException>()
            .WithMessage("DbUpdate message");
    }

    [Fact]
    public async Task DeleteRelation_RemovedFromToDb_IfDbNotThrowExceptions()
    {
        UserGroup? deletedObject = null;
        _context.Remove(Arg.Do<UserGroup>(o => deletedObject = o));

        await _sut.DeleteRelation<User, Group, UserGroup>(1, 2);

        deletedObject.Should().NotBeNull();
        deletedObject?.UserId.Should().Be(1);
        deletedObject?.GroupId.Should().Be(2);
    }

    [Fact]
    public async Task DeleteRelation_ShouldThrowException_IfDbUpdateConcurrencyExceptionWasThrown()
    {
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new DbUpdateConcurrencyException());

        await new Func<Task>(() => _sut.DeleteRelation<User, Group, UserGroup>(0, 0)).Should()
            .ThrowAsync<InfrastructureExceptions>()
            .WithMessage("cannot delete relation between object of type" +
                         " ECampus.Shared.Models.User with id=0 on between object of type" +
                         " ECampus.Shared.Models.Group with id=0 Error code: 404");
    }

    [Fact]
    public async Task DeleteRelation_ShouldThrowUnhandledException_IfExceptionWasThrown()
    {
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new DbUpdateException("DbUpdate message"));

        await new Func<Task>(() => _sut.DeleteRelation<User, Group, UserGroup>(0, 0)).Should()
            .ThrowAsync<UnhandledInfrastructureException>()
            .WithMessage(new UnhandledInfrastructureException(new Exception()).Message)
            .WithInnerException<UnhandledInfrastructureException, DbUpdateException>()
            .WithMessage("DbUpdate message");
    }
}