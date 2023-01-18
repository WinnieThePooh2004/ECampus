using UniversityTimetable.Infrastructure;
using UniversityTimetable.Infrastructure.DataAccessFacades;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.DataAccess;

namespace UniversityTimetable.Tests.Unit.BackEnd.Infrastructure.DataAccessFacadesTests;

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