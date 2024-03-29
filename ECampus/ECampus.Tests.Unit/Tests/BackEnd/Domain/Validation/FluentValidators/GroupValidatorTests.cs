﻿using ECampus.Domain.DataTransferObjects;
using ECampus.Validation;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Validation.FluentValidators;

public class GroupValidatorTests
{
    [Fact]
    public void PassedInvalidItem_ShouldHaveValidationError()
    {
        var invalidItem = new GroupDto { Name = "" };
        var validator = new GroupDtoValidator();

        var errors = validator.Validate(invalidItem).Errors.Select(e => e.ErrorMessage).ToList();
        var expectedErrors = new List<string>
        {
            "Please, enter name",
        };
        errors.Should().BeEquivalentTo(expectedErrors);

    }
}