using ECampus.Infrastructure.DataUpdateServices;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.DataUpdate;

public class PasswordChangeTests
{
    private readonly PasswordChange _sut;
    private readonly DbContext _context;
    private readonly User _testModel;

    public PasswordChangeTests()
    {
        _context = Substitute.For<DbContext>();
        _sut = new PasswordChange();
        _testModel = new User { Id = 10, Password = "OldPassword" };
        var data = new DbSetMock<User>(new List<User> { _testModel }).Object;
        _context.Set<User>().Returns(data);
    }

    [Fact]
    public async Task ChangePassword_ShouldThrowException_IfUserNotFoundById()
    {
        await new Func<Task>(() => _sut.ChangePassword(new PasswordChangeDto { UserId = 11 }, _context)).Should()
            .ThrowAsync<ObjectNotFoundByIdException>()
            .WithMessage(new ObjectNotFoundByIdException(typeof(User), 11).Message);

        await _context.DidNotReceive().SaveChangesAsync();
        _testModel.Password.Should().Be("OldPassword");
    }

    [Fact]
    public async Task ChangePassword_ShouldChangePassword_IfUserExists()
    {
        var passwordChange = new PasswordChangeDto { UserId = 10, NewPassword = "NewPassword" };

        await _sut.ChangePassword(passwordChange, _context);

        _testModel.Password.Should().Be(passwordChange.NewPassword);
    }
}