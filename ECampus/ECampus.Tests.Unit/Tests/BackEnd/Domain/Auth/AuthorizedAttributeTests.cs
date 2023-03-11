using ECampus.Domain.Enums;
using ECampus.WebApi.Metadata;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Auth;

public class AuthorizedAttributeTests
{
    [Fact]
    public void InitAttribute_ShouldAddAllRolesToBase()
    {
        var attribute = new AuthorizedAttribute(UserRole.Admin, UserRole.Guest);
        
        attribute.AuthenticationSchemes.Should().Be(JwtBearerDefaults.AuthenticationScheme);
        attribute.Roles.Should().Be("Admin,Guest");
    }

    [Fact]
    public void InitAttribute_ShouldInitSchemesAsCookies()
    {
        var attribute = new AuthorizedAttribute();

        attribute.AuthenticationSchemes.Should().Be(JwtBearerDefaults.AuthenticationScheme);
        attribute.Roles.Should().BeNullOrEmpty();
    }
}