using ECampus.Infrastructure;
using ECampus.Shared.Exceptions.InfrastructureExceptions;
using ECampus.Shared.Models;
using ECampus.Tests.Shared.Mocks.EntityFramework;

namespace ECampus.Tests.Unit.BackEnd.Infrastructure.AuthorizationDataAccess;

public class AuthorizationDataAccessTests
{
    private readonly ECampus.Infrastructure.Auth.AuthorizationDataAccess _sut;
    private readonly ApplicationDbContext _context = Substitute.For<ApplicationDbContext>();

    public AuthorizationDataAccessTests()
    {
        _sut = new ECampus.Infrastructure.Auth.AuthorizationDataAccess(_context);
    }

    [Fact]
    public async Task GetByEmail_ReturnsFromSet_IdObjectExists()
    {
        var expectedUser = new User{ Email = "abc@example.com"};
        var data = new List<User> { expectedUser };
        var set = new DbSetMock<User>(data).Object;
        _context.Users = set;

        var actualUser = await _sut.GetByEmailAsync("abc@example.com");

        actualUser.Should().Be(expectedUser);
    }

    [Fact]
    public async Task GetByEmail_ShouldThrowException_WhenQueryReturnsNull()
    {
        _context.Users = new DbSetMock<User>(new List<User>()).Object;

        await new Func<Task>(() => _sut.GetByEmailAsync("10")).Should().ThrowAsync<InfrastructureExceptions>()
            .WithMessage($"User with email 10 does not exist\nError code: 404");
    }
}