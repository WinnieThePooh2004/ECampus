using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.BaseServiceTests;

public sealed class BaseRepositoryFacultyDtoTests : BaseServiceTests<FacultyDto, Faculty>
{
    [Fact] protected override Task Create_ReturnsFromService_ServiceCalled_WhenNoValidationExceptions() => base.Create_ReturnsFromService_ServiceCalled_WhenNoValidationExceptions();

    [Fact] protected override Task Update_ReturnsFromService_WhenNoValidationExceptions() => base.Update_ReturnsFromService_WhenNoValidationExceptions();

    [Fact] protected override Task Delete_ShouldThrowException_WhenIdIsNull() => base.Delete_ShouldThrowException_WhenIdIsNull();

    [Fact] protected override Task Delete_ShouldCallService_WhenIdIsNotNull() => base.Delete_ShouldCallService_WhenIdIsNotNull();

    [Fact] protected override Task GetById_ShouldThrowException_WhenIdIsNull() => base.GetById_ShouldThrowException_WhenIdIsNull();

    [Fact] protected override Task GetById_ShouldReturnFromRepository_WhenIdIsNotNull() => base.GetById_ShouldReturnFromRepository_WhenIdIsNotNull();
    
    [Fact] protected override Task Create_ThrowsValidationExceptionWhenValidationErrorOccured() => base.Create_ThrowsValidationExceptionWhenValidationErrorOccured();

    [Fact] protected override Task Update_ThrowsValidationExceptionWhenValidationErrorOccured() => base.Update_ThrowsValidationExceptionWhenValidationErrorOccured();

}