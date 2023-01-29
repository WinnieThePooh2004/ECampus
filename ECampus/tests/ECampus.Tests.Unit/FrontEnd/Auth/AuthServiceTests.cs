using ECampus.FrontEnd.Auth;
using ECampus.FrontEnd.Requests.Interfaces;
using ECampus.Shared.Auth;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Exceptions.DomainExceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.FrontEnd.Auth;

public class AuthServiceTests
{
    private readonly AuthService _sut;
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly IAuthRequests _authRequests = Substitute.For<IAuthRequests>();

    public AuthServiceTests()
    {
        _sut = new AuthService(_httpContextAccessor, _authRequests);
    }

    [Fact]
    public async Task Login_ShouldThrowException_WhenHttpContextIsNull()
    {
        _httpContextAccessor.HttpContext = null;
        await new Func<Task>(() => _sut.Login("", "")).Should()
            .ThrowAsync<HttpContextNotFoundExceptions>()
            .WithMessage(new HttpContextNotFoundExceptions().Message);
    }

    [Fact]
    public async Task Login_ShouldLoginWithDataFromRequests_WhenHttpContextIsNotNull()
    {
        var properties = new AuthenticationProperties();
        _httpContextAccessor.HttpContext = Substitute.For<HttpContext>();
        _httpContextAccessor.HttpContext.RequestServices.Returns(Substitute.For<IServiceProvider>());
        _httpContextAccessor.HttpContext.RequestServices.GetService<IAuthenticationService>()
            .Returns(Substitute.For<IAuthenticationService>());
        _authRequests.LoginAsync(Arg.Is<LoginDto>(l => l.Email == "email" && l.Password == "password"))
            .Returns(new LoginResult());

        await _sut.Login("email", "password", properties);

        await _authRequests.Received(1).LoginAsync(Arg.Is<LoginDto>(l => l.Email == "email" && l.Password == "password"));
        
    }
}