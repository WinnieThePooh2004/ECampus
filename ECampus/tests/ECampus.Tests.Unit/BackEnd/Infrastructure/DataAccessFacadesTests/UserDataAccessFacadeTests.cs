using ECampus.Infrastructure;
using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Interfaces.DataAccess;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataAccessFacadesTests;

public class UserDataAccessFacadeTests
{
    private readonly UserDataAccessFacade _sut;
    private readonly ApplicationDbContext _context;
    private readonly IPasswordChange _passwordChange;

    public UserDataAccessFacadeTests()
    {
        _context = Substitute.For<ApplicationDbContext>();
        _passwordChange = Substitute.For<IPasswordChange>();
        _sut = new UserDataAccessFacade(_context, _passwordChange);
    }

    [Fact]
    public async Task ChangePassword_ShouldClassPasswordChange()
    {
        var passwordChange = new PasswordChangeDto();

        await _sut.ChangePassword(passwordChange);

        await _passwordChange.Received(1).ChangePassword(passwordChange, _context);
    }
}