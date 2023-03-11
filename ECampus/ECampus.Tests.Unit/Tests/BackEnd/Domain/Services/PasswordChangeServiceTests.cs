using ECampus.DataAccess.Contracts.DataAccess;
using ECampus.DataAccess.Contracts.DataSelectParameters;
using ECampus.Domain.DataTransferObjects;
using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Domain.Models;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Validation;
using ECampus.Services.Services;
using ECampus.Tests.Shared.DataFactories;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Services;

public class PasswordChangeServiceTests
{
    private readonly PasswordChangeService _sut;

    private readonly IUpdateValidator<PasswordChangeDto> _validator =
        Substitute.For<IUpdateValidator<PasswordChangeDto>>();

    private readonly IDataAccessFacade _dataAccess = Substitute.For<IDataAccessFacade>();

    private readonly Fixture _fixture = new();

    public PasswordChangeServiceTests()
    {
        _sut = new PasswordChangeService(_validator, MapperFactory.Mapper, _dataAccess);
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

        await _dataAccess.DidNotReceive().GetByIdAsync<User>(Arg.Any<int>());
        await _dataAccess.DidNotReceive().SaveChangesAsync();
    }

    [Fact]
    private async Task ChangePassword_ShouldReturnDtoBack_ShouldCallPasswordChange()
    {
        var errors = new ValidationResult();
        var user = new User { Id = new Random().Next() };
        var passwordChange = _fixture.Create<PasswordChangeDto>();
        var users = new DbSetMock<User>(user).Object;
        _dataAccess.GetByParameters<User, PureByIdParameters<User>>(Arg.Any<PureByIdParameters<User>>()).Returns(users);
        _validator.ValidateAsync(passwordChange).Returns(errors);

        var result = await _sut.ChangePassword(passwordChange);

        result.Id.Should().Be(user.Id);
        user.Password.Should().Be(passwordChange.NewPassword);
        await _dataAccess.Received(1).SaveChangesAsync();
    }
}