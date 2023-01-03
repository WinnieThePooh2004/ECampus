using FluentValidation;
using FluentValidation.Results;
using UniversityTimetable.Domain.Validation.UpdateValidators;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Validation.UpdateValidators;

public class UpdateValidatorTests
{
    private readonly IValidator<AuditoryDto> _fluentValidator;
    private readonly UpdateValidator<AuditoryDto> _sut;
    private readonly Fixture _fixture = new();

    public UpdateValidatorTests()
    {
        _fluentValidator = Substitute.For<IValidator<AuditoryDto>>();
        _sut = new UpdateValidator<AuditoryDto>(_fluentValidator);
    }

    [Fact]
    public async Task Validate_ReturnsFromFluentValidator()
    {
        var errors = _fixture.CreateMany<KeyValuePair<string, string>>(10).ToList();
        var validationResult = new ValidationResult(errors
            .Select(e => new ValidationFailure(e.Key, e.Value)));
        var auditory = new AuditoryDto();
        _fluentValidator.ValidateAsync(auditory).Returns(validationResult);

        var actualErrors = await _sut.ValidateAsync(auditory);

        actualErrors.Should().Contain(errors);
    }
}