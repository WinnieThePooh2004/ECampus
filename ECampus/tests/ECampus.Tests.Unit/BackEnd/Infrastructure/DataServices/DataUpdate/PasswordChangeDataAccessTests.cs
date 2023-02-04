using ECampus.Infrastructure.DataAccessFacades;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.Mocks.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.DataServices.DataUpdate;

public class PasswordChangeDataAccessTests
{
    private readonly PasswordChangeDataAccess _sut;
    private readonly DbContext _context;
    private readonly User _testModel;

    public PasswordChangeDataAccessTests()
    {
        _context = Substitute.For<DbContext>();
        _sut = new PasswordChangeDataAccess(_context);
        _testModel = new User { Id = 10 };
        var data = new DbSetMock<User>(new List<User> { _testModel }).Object;
        _context.Set<User>().Returns(data);
    }

    [Fact]
    public async Task ChangePassword_ShouldThrowException_IfUserNotFoundById()
    {
        await new Func<Task>(() => _sut.GetUserAsync(11)).Should()
            .ThrowAsync<ObjectNotFoundByIdException>()
            .WithMessage(new ObjectNotFoundByIdException(typeof(User), 11).Message);
    }

    [Fact]
    public async Task ChangePassword_ShouldReturnUser_IfUserExists()
    {
        var result = await _sut.GetUserAsync(10);

        result.Should().Be(_testModel);
    }

    [Fact]
    public async Task SaveChanges_ShouldSaveChangesInContext()
    {
        await _sut.SaveChangesAsync();

        await _context.Received(1).SaveChangesAsync();
    }
}