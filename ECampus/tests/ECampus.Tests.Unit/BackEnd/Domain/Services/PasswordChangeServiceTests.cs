﻿using ECampus.Contracts.DataAccess;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Services.Services;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using ECampus.Shared.Models;
using ECampus.Shared.Validation;
using ECampus.Tests.Shared.DataFactories;

namespace ECampus.Tests.Unit.BackEnd.Domain.Services;

public class PasswordChangeServiceTests
{
    private readonly PasswordChangeService _sut;

    private readonly IUpdateValidator<PasswordChangeDto> _validator =
        Substitute.For<IUpdateValidator<PasswordChangeDto>>();

    private readonly IPasswordChangeDataAccess _dataAccess = Substitute.For<IPasswordChangeDataAccess>();

    private readonly Fixture _fixture = new();

    public PasswordChangeServiceTests()
    {
        _sut = new PasswordChangeService(_dataAccess, _validator, MapperFactory.Mapper);
    }

    [Fact]
    public async Task ValidatePasswordChange_ShouldReturnFromPasswordChange()
    {
        var passwordChange = new PasswordChangeDto();
        var errors = new ValidationResult(_fixture.CreateMany<ValidationError>(10).ToList());
        _validator.ValidateAsync(passwordChange).Returns(errors);

        var result = await _sut.ValidatePasswordChange(passwordChange);

        ((object)result).Should().Be(errors);
    }

    [Fact]
    private async Task ChangePassword_ShouldThrowValidationException_WhenHasValidationError()
    {
        var errors = new ValidationResult(new ValidationError("", ""));
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        _validator.ValidateAsync(passwordChange).Returns(errors);

        await new Func<Task>(() => _sut.ChangePassword(passwordChange)).Should().ThrowAsync<ValidationException>()
            .WithMessage(new ValidationException(typeof(PasswordChangeDto), errors).Message);

        await _dataAccess.DidNotReceive().ChangePassword(Arg.Any<PasswordChangeDto>());
    }

    [Fact]
    private async Task ChangePassword_ShouldReturnDtoBack_ShouldCallPasswordChange()
    {
        var errors = new ValidationResult();
        var user = new User { Id = new Random().Next() };
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        _dataAccess.ChangePassword(passwordChange).Returns(user);
        _validator.ValidateAsync(passwordChange).Returns(errors);

        var result = await _sut.ChangePassword(passwordChange);

        result.Id.Should().Be(user.Id);
        await _dataAccess.Received(1).ChangePassword(passwordChange);
    }
}