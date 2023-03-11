using ECampus.Services.Validation.FluentValidators;
using ECampus.Shared.DataTransferObjects;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Validation.FluentValidators;

public class ClassValidatorTests
{
    [Fact]
    public void PassedInvalidItem_ShouldHaveValidationError()
    {
        var invalidItem = new ClassDto { TeacherId = 0, GroupId = 0, AuditoryId = 0, SubjectId = 0 };
        var validator = new ClassDtoValidator();

        var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
        var expectedErrors = new List<string>
        {
            "Please, select auditory",
            "Please, select group",
            "Please, select teacher",
            "Please, select subject"
        };
        errors.Should().BeEquivalentTo(expectedErrors);
    }}