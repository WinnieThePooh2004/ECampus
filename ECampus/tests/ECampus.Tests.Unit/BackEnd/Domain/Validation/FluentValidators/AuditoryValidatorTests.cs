using ECampus.Domain.Validation.FluentValidators;
using ECampus.Shared.DataTransferObjects;

namespace ECampus.Tests.Unit.BackEnd.Domain.Validation.FluentValidators;

public class AuditoryValidatorTests
{
    [Fact]
    public void PassedInvalidItem_ShouldHaveValidationError_ServiceCalled()
    {
        var invalidItem = new AuditoryDto { Name = "", Building = "" };
        var validator = new AuditoryDtoValidator();

        var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
        var expectedErrors = new List<string>
        {
            "Please, enter name",
            "Please, enter building`s name"
        };
        errors.Should().BeEquivalentTo(expectedErrors);
    }
}