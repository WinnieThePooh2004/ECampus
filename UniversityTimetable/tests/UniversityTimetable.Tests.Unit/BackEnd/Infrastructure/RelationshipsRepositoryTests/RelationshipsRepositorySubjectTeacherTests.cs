using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.Models.RelationModels;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.RelationshipsRepositoryTests;

public class RelationshipsRepositorySubjectTeacherTests : RelationshipsRepositoryTests<Subject, Teacher, SubjectTeacher>
{
    [Fact] protected override Task AddRelation_ShouldAddToDb_IfDbNotThrowExceptions() => base.AddRelation_ShouldAddToDb_IfDbNotThrowExceptions();

    [Fact] protected override Task AddRelation_ShouldThrowException_IfErrorOccuredWhileSaveChanges() => base.AddRelation_ShouldThrowException_IfErrorOccuredWhileSaveChanges();

    [Fact] protected override Task DeleteRelation_RemovedFromToDb_IfDbNotThrowExceptions() => base.DeleteRelation_RemovedFromToDb_IfDbNotThrowExceptions();

    [Fact] protected override Task DeleteRelation_ShouldThrowException_IfErrorOccuredWhileSaveChanges() => base.DeleteRelation_ShouldThrowException_IfErrorOccuredWhileSaveChanges();

    [Fact] protected override void CreateRelationModels_ShouldAddToModel() => base.CreateRelationModels_ShouldAddToModel();

    [Fact] protected override Task UpdateRelations_ShouldUpdateRelations() => base.UpdateRelations_ShouldUpdateRelations();
}