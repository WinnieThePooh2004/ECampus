using Microsoft.Extensions.Logging;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.Relationships;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.Relationships;

public class RelationsDataAccessTests
{
    private readonly ApplicationDbContext _context;
    private readonly RelationsDataAccess _sut;

    public RelationsDataAccessTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _sut = new RelationsDataAccess(Substitute.For<ILogger<RelationsDataAccess>>());
    }
    
    [Fact]
    public async Task AddRelation_ShouldAddToDb_IfDbNotThrowExceptions()
    {
        await _sut.CreateRelation<UserGroup, User, Group>(1, 2, _context);
    
        _context.Received(1).Add(Arg.Any<object>());
    }

    [Fact]
    public async Task AddRelation_ShouldThrowException_IfErrorOccuredWhileSaveChanges()
    {
        _context.SaveChangesAsync().Returns(0).AndDoes(_ => throw new Exception("Some message"));
    
        await new Func<Task>(() => _sut.CreateRelation<UserGroup, User, Group>(0, 0, _context)).Should()
            .ThrowAsync<InfrastructureExceptions>()
            .WithMessage("Some message\nError code: 404");
    }

    [Fact]
    public async Task DeleteRelation_RemovedFromToDb_IfDbNotThrowExceptions()
    {
        await _sut.DeleteRelation<UserGroup, User, Group>(1, 2, _context);
    
        _context.Received(1).Remove(Arg.Any<object>());
    }

    [Fact]
    public async Task DeleteRelation_ShouldThrowException_IfErrorOccuredWhileSaveChanges()
    {
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new Exception("Some message"));
    
        await new Func<Task>(() => _sut.DeleteRelation<UserGroup, User, Group>(0, 0, _context)).Should()
            .ThrowAsync<InfrastructureExceptions>()
            .WithMessage("Some message\nError code: 404");
    }

}