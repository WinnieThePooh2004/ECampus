using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using UniversityTimetable.FrontEnd.Extensions;
using UniversityTimetable.FrontEnd.Requests.Interfaces;
using UniversityTimetable.Shared.Auth;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Tests.Unit.FrontEnd.Requests;

public class UserRequestsExtensionsTests
{
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly HttpContext _httpContext = Substitute.For<HttpContext>();
    private readonly IBaseRequests<UserDto> _sut = Substitute.For<IBaseRequests<UserDto>>();

    public UserRequestsExtensionsTests()
    {
        _httpContextAccessor.HttpContext.Returns(_httpContext);
    }
    
    [Fact]
    public async Task GetCurrentUser_ShouldThrowException_IfUserDoesNotHaveIdClaim()
    {
        _httpContext.User.Returns(new ClaimsPrincipal());

        await new Func<Task>(() => _sut.GetCurrentUserAsync(_httpContextAccessor)).Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetCurrentUser_ShouldThrowException_IfIdClaimIsNotNumber()
    {
        _httpContext.User.Returns(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(CustomClaimTypes.Id, "abc")
        })));

        await new Func<Task>(() => _sut.GetCurrentUserAsync(_httpContextAccessor)).Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetCurrentUser_ShouldThrowException_IfHttpContextIsNull()
    {
        _httpContextAccessor.HttpContext = null;

        await new Func<Task>(() => _sut.GetCurrentUserAsync(_httpContextAccessor)).Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetCurrentUser_ShouldThrowException_IfHttpIdentityIsNull()
    {
        _httpContext.User.Returns(Substitute.For<ClaimsPrincipal>());
        _httpContext.User.Identity.Returns(null as IIdentity);

        await new Func<Task>(() => _sut.GetCurrentUserAsync(_httpContextAccessor)).Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetCurrentUser_ShouldThrowException_IfUserIsNull()
    {
        await new Func<Task>(() => _sut.GetCurrentUserAsync(_httpContextAccessor)).Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetCurrentUser_ShouldThrowException_IfNotAuthenticated()
    {
        var identity = Substitute.For<IIdentity>();
        identity.IsAuthenticated.Returns(false);
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _httpContextAccessor.HttpContext?.User.Returns(claimsPrincipal);

        await new Func<Task>(() => _sut.GetCurrentUserAsync(_httpContextAccessor)).Should()
            .ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task GetCurrentUser_ShouldCallRequests_WhenNoExceptionWasThrown()
    {
        var user = new UserDto();
        _sut.GetByIdAsync(10).Returns(user);
        _httpContextAccessor.HttpContext?.User.Returns(new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new(CustomClaimTypes.Id, "10")
        }, CookieAuthenticationDefaults.AuthenticationScheme)));

        var result = await _sut.GetCurrentUserAsync(_httpContextAccessor);

        result.Should().Be(user);
        await _sut.Received(1).GetByIdAsync(10);
    }
}