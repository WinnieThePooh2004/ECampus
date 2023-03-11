using System.Security.Claims;
using ECampus.Domain.Auth;
using ECampus.Domain.QueryParameters;
using ECampus.Domain.Validation;
using ECampus.Services.Validation.ParametersValidators;
using Microsoft.AspNetCore.Http;

namespace ECampus.Tests.Unit.Tests.BackEnd.Domain.Validation.ParametersValidation;

public class CourseSummaryParametersValidatorTests
{
    private readonly CourseSummaryParametersValidator _sut;
    private readonly ClaimsPrincipal _user = Substitute.For<ClaimsPrincipal>();

    public CourseSummaryParametersValidatorTests()
    {
        var contextAccessor = Substitute.For<IHttpContextAccessor>();
        contextAccessor.HttpContext = Substitute.For<HttpContext>();
        contextAccessor.HttpContext.User.Returns(_user);
        _sut = new CourseSummaryParametersValidator(contextAccessor);
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenStudentIdClaimIsNotANumber()
    {
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "abc"));

        var validationResult = await _sut.ValidateAsync(new CourseSummaryParameters());

        validationResult.Should().BeEquivalentTo(new List<ValidationError>
            { new("user", $"Claim '{CustomClaimTypes.StudentId}' must be a number, not 'abc'") });
    }

    [Fact]
    public async Task Validate_ShouldAddError_WhenStudentIdDoesNotMatches()
    {
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "10"));

        var validationResult = await _sut.ValidateAsync(new CourseSummaryParameters { StudentId = 11 });

        validationResult.Should().BeEquivalentTo(new List<ValidationError>
            { new("StudentId", "You profile`s student id does not matches provided") });
    }
    
    [Fact]
    public async Task Validate_ShouldReturnEmpty_WhenStudentIdMatches()
    {
        _user.FindFirst(CustomClaimTypes.StudentId).Returns(new Claim("", "10"));

        var validationResult = await _sut.ValidateAsync(new CourseSummaryParameters { StudentId = 10 });

        validationResult.Should().BeEmpty();
    }
}