using Microsoft.Extensions.Logging;
using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.Relationships;
using UniversityTimetable.Shared.Interfaces.Data.Models;
using UniversityTimetable.Tests.Shared.Mocks;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.RelationshipsRepositoryTests;

public abstract class RelationshipsRepositoryTests<TLeftTable, TRightTable, TRelations>
    where TLeftTable : class, IModel, IModelWithManyToManyRelations<TRightTable, TRelations>, new()
    where TRightTable : class, IModel, new()
    where TRelations : class, IRelationModel<TLeftTable, TRightTable>, new()

{
    private readonly RelationshipsDataAccess<TLeftTable, TRightTable, TRelations> _dataAccess;
    private readonly ApplicationDbContext _context;

    protected RelationshipsRepositoryTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _dataAccess = new RelationshipsDataAccess<TLeftTable, TRightTable, TRelations>();
    }
    // protected virtual async Task AddRelation_ShouldAddToDb_IfDbNotThrowExceptions()
    // {
    //     await _dataAccess.CreateRelation(1, 2);
    //
    //     _context.Received(1).Add(Arg.Any<TRelations>());
    // }

    // protected virtual async Task AddRelation_ShouldThrowException_IfErrorOccuredWhileSaveChanges()
    // {
    //     _context.SaveChangesAsync().Returns(0).AndDoes(_ => throw new Exception("Some message"));
    //
    //     await new Func<Task>(() => _dataAccess.CreateRelation(0, 0)).Should()
    //         .ThrowAsync<InfrastructureExceptions>()
    //         .WithMessage("Some message\nError code: 404");
    // }

    // protected virtual async Task DeleteRelation_RemovedFromToDb_IfDbNotThrowExceptions()
    // {
    //     await _dataAccess.DeleteRelation(1, 2);
    //
    //     _context.Received(1).Remove(Arg.Any<TRelations>());
    // }

    // protected virtual async Task DeleteRelation_ShouldThrowException_IfErrorOccuredWhileSaveChanges()
    // {
    //     _context.SaveChangesAsync().Returns(1).AndDoes(_ => throw new Exception("Some message"));
    //
    //     await new Func<Task>(() => _dataAccess.DeleteRelation(0, 0)).Should()
    //         .ThrowAsync<InfrastructureExceptions>()
    //         .WithMessage("Some message\nError code: 404");
    // }

    protected virtual void CreateRelationModels_ShouldAddToModel()
    {
        var model = new TLeftTable
        {
            Id = 3,
            RelatedModels = new List<TRightTable>()
        };
        model.RelatedModels.AddRange(new List<TRightTable>
        {
            new() { Id = 1 },
            new() { Id = 2 }
        });

        _dataAccess.CreateRelationModels(model);

        model.RelationModels.Should().BeEquivalentTo(new List<TRelations>
        {
            new() { LeftTableId = 3, RightTableId = 1 },
            new() { LeftTableId = 3, RightTableId = 2 }
        }, opt => opt.ComparingByMembers<TRelations>());
    }

    protected virtual async Task UpdateRelations_ShouldUpdateRelations()
    {
        var dataSource = new List<TRelations>
        {
            new() { LeftTableId = 10, RightTableId = 3 },
            new() { LeftTableId = 10, RightTableId = 5 },
        };
        List<TRelations>? deleted = null;
        List<TRelations>? added = null;
        var currentRelationModels = new DbSetMock<TRelations>(dataSource).Object;
        _context.Set<TRelations>().Returns(currentRelationModels);
        _context.RemoveRange(Arg.Do<IEnumerable<object>>(list => deleted = list
            .Select(i => (TRelations)i).ToList()));
        _context.AddRange(Arg.Do<IEnumerable<object>>(list => added = list
            .Select(i => (TRelations)i).ToList()));
        var updatedEntity = new TLeftTable
        {
            Id = 10,
            RelatedModels = new List<TRightTable>
            {
                new() { Id = 3 },
                new() { Id = 4 }
            }
        };

        await _dataAccess.UpdateRelations(updatedEntity, _context);

        deleted.Should().BeEquivalentTo(new List<TRelations>
        {
            new() { LeftTableId = 10, RightTableId = 5 }
        });

        added.Should().BeEquivalentTo(new List<TRelations>
        {
            new() { LeftTableId = 10, RightTableId = 4 }
        });
    }
}