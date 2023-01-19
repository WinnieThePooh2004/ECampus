using FluentValidation;
using FluentValidation.Results;
using UniversityTimetable.Domain.Validation.UniversalValidators;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Validation;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace UniversityTimetable.Tests.Unit.BackEnd.Domain.Validation.UniversalValidators;

public class UniversalValidatorTests
{
    private readonly FluentValidatorWrapper<FacultyDto> _sut;
    private readonly IValidator<FacultyDto> _fluentValidator;

    public UniversalValidatorTests()
    {
        _fluentValidator = Substitute.For<IValidator<FacultyDto>>();
        _sut = new FluentValidatorWrapper<FacultyDto>(_fluentValidator);
    }

    [Fact]
    public async Task Validate_ReturnsFromFluentValidator()
    {
        var errors = new Fixture().CreateMany<KeyValuePair<string, string>>(10).ToList();
        var validationResult = new ValidationResult();
        validationResult.Errors.AddRange(errors
            .Select(e => new ValidationFailure(e.Key, e.Value)));
        var faculty = new FacultyDto();
        _fluentValidator.ValidateAsync(faculty).Returns(validationResult);

        var actualResult = await _sut.ValidateAsync(faculty);

        actualResult.GetAllErrors().Should().Contain(errors.Select(e 
            => new ValidationError(e.Key, e.Value)));
    }
}