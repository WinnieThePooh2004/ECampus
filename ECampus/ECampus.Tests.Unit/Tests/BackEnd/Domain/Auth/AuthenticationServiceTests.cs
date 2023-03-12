using System.Security.Claims;
using ECampus.Domain.Enums;
using ECampus.Domain.Exceptions.DomainExceptions;
using ECampus.Services.Services.Auth;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Auth;

public class AuthenticationServiceTests
{
    private readonly AuthenticationService _sut;
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly HttpContext _httpContext = Substitute.For<HttpContext>();

    public AuthenticationServiceTests()
    {
        _sut = new AuthenticationService(_httpContextAccessor);
        _httpContextAccessor.HttpContext = _httpContext;
    }

    [Fact]
    private void VerifyUser_ShouldThrowException_WhenHttpContextIsNull()
    {
        _httpContextAccessor.HttpContext = null;

        new Action(() => _sut.VerifyUser(10)).Should().Throw<HttpContextNotFoundExceptions>()
            .WithMessage(new HttpContextNotFoundExceptions().Message);
    }

    [Fact]
    public void VerifyUser_ShouldThrowException_WhenUserNotAuthorized()
    {
        var user = new ClaimsPrincipal();
        _httpContext.User.Returns(user);
        new Action(() => _sut.VerifyUser(10)).Should().Throw<DomainException>()
            .WithMessage("Cannot verify user than is not authenticated\nError code: 401");
    }

    [Fact]
    public void VerifyUser_ShouldThrowExceptions_WhenIdClaimNotFound()
    {
        var identity = new ClaimsIdentity(new List<Claim>(), "authType");
        _httpContextAccessor.HttpContext?.User.Returns(new ClaimsPrincipal(identity));

        new Action(() => _sut.VerifyUser(10)).Should().Throw<DomainException>()
            .WithMessage("User claims should have Claim with user id\nError code: 401");
    }

    [Fact]
    public void VerifyUser_ShouldThrowException_WhenIdInClaimsIsNotNumber()
    {
        var identity = new ClaimsIdentity(new List<Claim> { new(ClaimTypes.Sid, "abc") },
            "authType");
        _httpContextAccessor.HttpContext?.User.Returns(new ClaimsPrincipal(identity));

        new Action(() => _sut.VerifyUser(10)).Should().Throw<DomainException>()
            .WithMessage("User claims should have Claim with user id\nError code: 401");
    }
    
    [Fact]
    public void VerifyUser_ShouldThrowException_WhenIdIsNotSameAsProvidedAndUserIsNotAdmin()
    {
        var identity = new ClaimsIdentity(new List<Claim> { new(ClaimTypes.Sid, "1") },
            "authType");
        _httpContextAccessor.HttpContext?.User.Returns(new ClaimsPrincipal(identity));

        new Action(() => _sut.VerifyUser(10)).Should().Throw<DomainException>()
            .WithMessage("To do this action you must be registered with user" +
                         " id same as requested or be admin\nError code: 403");
    }
    
    [Fact]
    public void VerifyUser_ShouldNotThrowException_WhenIdIsNotSameAsProvidedAndUserIsAdmin()
    {
        var identity = new ClaimsIdentity(new List<Claim>
            {
                new(ClaimTypes.Sid, "1"),
                new(ClaimTypes.Role, nameof(UserRole.Admin))
            },
            "authType");
        _httpContextAccessor.HttpContext?.User.Returns(new ClaimsPrincipal(identity));

        _sut.VerifyUser(10);
    }
    
    [Fact]
    public void VerifyUser_ShouldNotThrowException_WhenIdIsSameAsProvided()
    {
        var identity = new ClaimsIdentity(new List<Claim> { new(ClaimTypes.Sid, "10") },
            "authType");
        _httpContextAccessor.HttpContext?.User.Returns(new ClaimsPrincipal(identity));
        
        _sut.VerifyUser(10);
    }
}