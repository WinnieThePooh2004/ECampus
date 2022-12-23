using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.BaseServiceTests;

public class BaseRepositoryUserDtoTests : BaseServiceTests<UserDto, User>
{
    [Fact] protected override Task Create_ReturnsFromService_ServiceCalled() => base.Create_ReturnsFromService_ServiceCalled();

    [Fact] protected override Task Update_ReturnsFromService() => base.Update_ReturnsFromService();

    [Fact] protected override Task Delete_ShouldThrowException_WhenIdIsNull() => base.Delete_ShouldThrowException_WhenIdIsNull();

    [Fact] protected override Task Delete_ShouldCallService_WhenIdIsNotNull() => base.Delete_ShouldCallService_WhenIdIsNotNull();

    [Fact] protected override Task GetById_ShouldThrowException_WhenIdIsNull() => base.GetById_ShouldThrowException_WhenIdIsNull();

    [Fact] protected override Task GetById_ShouldReturnFromRepository_WhenIdIsNotNull() => base.GetById_ShouldReturnFromRepository_WhenIdIsNotNull();
}