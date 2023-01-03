using UniversityTimetable.Domain.Validation;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Data.Validation;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Validation;

public class ValidationFacadeTests
{
    private readonly ICreateValidator<AuditoryDto> _createValidator;
    private readonly IUpdateValidator<AuditoryDto> _updateValidator;
    private readonly ValidationFacade<AuditoryDto> _validationFacade;
    private readonly Fixture _fixture = new();

    public ValidationFacadeTests()
    {
        _updateValidator = Substitute.For<IUpdateValidator<AuditoryDto>>();
        _createValidator = Substitute.For<ICreateValidator<AuditoryDto>>();

        _validationFacade = new ValidationFacade<AuditoryDto>(_updateValidator, _createValidator);
    }

    [Fact]
    public async Task ValidateCreate_ReturnsFromCreateValidator()
    {
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>().ToList();
        var auditory = new AuditoryDto();
        _createValidator.ValidateAsync(auditory).Returns(errors);

        var actualErrors = await _validationFacade.ValidateCreate(auditory);

        actualErrors.Should().Contain(errors);
        await _createValidator.Received(1).ValidateAsync(auditory);
    }

    [Fact]
    public async Task ValidateCreate_ShouldAddMessage_WhenIdIsNotZero()
    {
        var errors = new List<KeyValuePair<string, string>>();
        var auditory = new AuditoryDto { Id = -10 };
        _createValidator.ValidateAsync(auditory).Returns(errors);

        var actualErrors = await _validationFacade.ValidateCreate(auditory);

        await _createValidator.Received(1).ValidateAsync(auditory);
        actualErrors.Should()
            .ContainEquivalentOf(KeyValuePair.Create("Id", "Cannot add object to database if its id is not 0"));
    }

    [Fact]
    public async Task ValidateUpdate_ShouldAddMessage_WhenIdLessThanZero()
    {
        var errors = new List<KeyValuePair<string, string>>();
        var auditory = new AuditoryDto { Id = -10 };
        _updateValidator.ValidateAsync(auditory).Returns(errors);

        var actualErrors = await _validationFacade.ValidateUpdate(auditory);

        await _updateValidator.Received(1).ValidateAsync(auditory);
        actualErrors.Should()
            .ContainEquivalentOf(KeyValuePair.Create("Id", "Cannot update object if its id is less or equal 0"));
    }

    [Fact]
    public async Task ValidateUpdate_ReturnsFromUpdateValidator()
    {
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>().ToList();
        var auditory = new AuditoryDto { Id = 10 };
        _updateValidator.ValidateAsync(auditory).Returns(errors);

        var actualErrors = await _validationFacade.ValidateUpdate(auditory);

        await _updateValidator.Received(1).ValidateAsync(auditory);
        actualErrors.Should()
            .NotContainEquivalentOf(KeyValuePair.Create("Id", "Cannot update object if it`s id is less or equal 0"));
    }
}