using System.Security.Claims;
using Bunit;
using ECampus.Domain.Auth;
using ECampus.Domain.DataTransferObjects;
using ECampus.FrontEnd.Components.EditForms;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ECampus.Tests.Unit.Tests.FrontEnd.Components.EditForms;

public class PasswordEditFormTests
{
    private readonly TestContext _context = new();
    private readonly IValidator<PasswordChangeDto> _validator = Substitute.For<IValidator<PasswordChangeDto>>();
    private readonly IHttpContextAccessor _httpContextAccessor = Substitute.For<IHttpContextAccessor>();
    private readonly HttpContext _httpContext = Substitute.For<HttpContext>();

    public PasswordEditFormTests()
    {
        _httpContextAccessor.HttpContext.Returns(_httpContext);
        _context.Services.AddSingleton(_httpContextAccessor);
        _context.Services.AddSingleton(_validator);
    }

    [Fact]
    public void Initialize_ShouldThrowException_WhenNoHttpContextFound()
    {
        _httpContextAccessor.HttpContext = null;
        new Action(() => _context.RenderComponent<PasswordEditForm>()).Should()
            .Throw<UnauthorizedAccessException>();
    }

    [Fact]
    public void Initialize_ShouldThrowException_WhenNoIdClaimFound()
    {
        _httpContext.User.Returns(new ClaimsPrincipal());
        new Action(() => _context.RenderComponent<PasswordEditForm>()).Should()
            .Throw<UnauthorizedAccessException>();
    }

    [Fact]
    public void Initialize_ShouldThrowException_WhenNoIdClaimIsNotNumber()
    {
        _httpContext.User.Returns(
            new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new(CustomClaimTypes.Id, "ms") })));
        new Action(() => _context.RenderComponent<PasswordEditForm>()).Should()
            .Throw<UnauthorizedAccessException>();
    }

    [Fact]
    public void Initialize_ShouldInitModelIdFromClaims_WhenNoExceptionThrown()
    {
        var model = new PasswordChangeDto();
        _httpContext.User.Returns(
            new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new(CustomClaimTypes.Id, "10") })));
        new Action(() => _context.RenderComponent<PasswordEditForm>(options => 
                options.Add(form => form.Model, model))).Should()
            .NotThrow();

        model.UserId.Should().Be(10);
    }
}