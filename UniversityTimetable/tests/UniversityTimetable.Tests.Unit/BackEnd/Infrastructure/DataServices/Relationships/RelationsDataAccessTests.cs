﻿using Microsoft.Extensions.Logging;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.Relationships;
using UniversityTimetable.Shared.Exceptions.InfrastructureExceptions;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataServices.Relationships;

public class RelationsDataAccessTests
{
    private readonly ApplicationDbContext _context;
    private readonly RelationsDataAccess<User, Group, UserGroup> _sut;

    public RelationsDataAccessTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _sut = new RelationsDataAccess<User, Group, UserGroup>(
            Substitute.For<ILogger<RelationsDataAccess<User, Group, UserGroup>>>(),
            new RelationshipsHandler<User, Group, UserGroup>());
    }

    [Fact]
    public async Task AddRelation_ShouldAddToDb_IfDbNotThrowExceptions()
    {
        UserGroup? deletedObject = null;
        _context.Add(Arg.Do<object>(o => deletedObject = (UserGroup)o));
        
        await _sut.CreateRelation(1, 2, _context);
        
        deletedObject?.UserId.Should().Be(1);
        deletedObject?.GroupId.Should().Be(2);
    }

    [Fact]
    public async Task AddRelation_ShouldThrowException_IfErrorOccuredWhileSaveChanges()
    {
        _context.SaveChangesAsync().Returns(0).AndDoes(_ => throw new Exception("Some message"));

        await new Func<Task>(() => _sut.CreateRelation(0, 0, _context)).Should()
            .ThrowAsync<InfrastructureExceptions>()
            .WithMessage("Some message\nError code: 404");
    }

    [Fact]
    public async Task DeleteRelation_RemovedFromToDb_IfDbNotThrowExceptions()
    {
        UserGroup? deletedObject = null;
        _context.Remove(Arg.Do<UserGroup>(o => deletedObject = o));
        
        await _sut.DeleteRelation(1, 2, _context);

        deletedObject.Should().NotBeNull();
        deletedObject?.UserId.Should().Be(1);
        deletedObject?.GroupId.Should().Be(2);
    }

    [Fact]
    public async Task DeleteRelation_ShouldThrowException_IfErrorOccuredWhileSaveChanges()
    {
        _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new Exception("Some message"));

        await new Func<Task>(() => _sut.DeleteRelation(0, 0, _context)).Should()
            .ThrowAsync<InfrastructureExceptions>()
            .WithMessage("Some message\nError code: 404");
    }
}