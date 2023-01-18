using UniversityTimetable.Domain.Services;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Exceptions.DomainExceptions;
using UniversityTimetable.Shared.Interfaces.Domain;
using UniversityTimetable.Shared.Interfaces.Domain.Validation;
using UniversityTimetable.Shared.Validation;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Services;

public class ServiceWithUpdateValidationTests
{
    private readonly ServiceWithUpdateValidation<GroupDto> _sut;
    private readonly IUpdateValidator<GroupDto> _validator = Substitute.For<IUpdateValidator<GroupDto>>();
    private readonly IBaseService<GroupDto> _baseService = Substitute.For<IBaseService<GroupDto>>();
    private readonly Fixture _fixture = new();

    public ServiceWithUpdateValidationTests()
    {
        _sut = new ServiceWithUpdateValidation<GroupDto>(_baseService, _validator);
    }

    [Fact]
    public async Task GetById_ReturnsFromBaseService()
    {
        var group = _fixture.Create<GroupDto>();
        _baseService.GetByIdAsync(10).Returns(group);

        var result = await _sut.GetByIdAsync(10);

        result.Should().Be(group);
    }

    [Fact]
    public async Task Create_ReturnsFromBaseService()
    {
        var group = _fixture.Create<GroupDto>();
        _baseService.CreateAsync(group).Returns(group);

        var result = await _sut.CreateAsync(group);

        result.Should().Be(group);
    }

    [Fact]
    public async Task Delete_CallsBaseService()
    {
        await _sut.DeleteAsync(10);

        await _baseService.Received().DeleteAsync(10);
    }

    [Fact]
    public async Task Update_ReturnsFromBaseService_WhenNoValidationErrors()
    {
        var group = _fixture.Create<GroupDto>();
        var errors = new ValidationResult();
        _validator.ValidateAsync(group).Returns(errors);
        _baseService.UpdateAsync(group).Returns(group);

        var result = await _sut.UpdateAsync(group);

        result.Should().Be(group);
    }

    [Fact]
    public async Task Update_ShouldThrowException_WhenValidationErrorOccured()
    {
        var group = _fixture.Create<GroupDto>();
        _validator.ValidateAsync(group)
            .Returns(new ValidationResult(_fixture.CreateMany<ValidationError>(10).ToList()));

        await new Func<Task>(() => _sut.UpdateAsync(group)).Should().ThrowAsync<ValidationException>();

        await _baseService.DidNotReceive().UpdateAsync(group);
    }
}