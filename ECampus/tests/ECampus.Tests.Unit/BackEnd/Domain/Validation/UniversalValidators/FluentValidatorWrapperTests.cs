using ECampus.Domain.Validation.UniversalValidators;
using ECampus.Domain.Validation.UpdateValidators;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Validation;
using FluentValidation;
using FluentValidation.Results;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.UniversalValidators;

public class FluentValidatorWrapperTests
{
    private readonly FluentValidatorWrapper<FacultyDto> _sut;
    private readonly IValidator<FacultyDto> _fluentValidator;

    public FluentValidatorWrapperTests()
    {
        _fluentValidator = Substitute.For<IValidator<FacultyDto>>();
        _sut = new UpdateFluentValidatorWrapper<FacultyDto>(_fluentValidator);
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

        actualResult.ToList().Should().Contain(errors.Select(e 
            => new ValidationError(e.Key, e.Value)));
    }
}