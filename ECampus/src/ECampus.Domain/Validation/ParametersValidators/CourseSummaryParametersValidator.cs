using System.Security.Claims;
using ECampus.Domain.Interfaces.Validation;
using ECampus.Shared.Auth;
using ECampus.Shared.QueryParameters;
using ECampus.Shared.Validation;
using Microsoft.AspNetCore.Http;

namespace ECampus.Domain.Validation.ParametersValidators;

public class CourseSummaryParametersValidator : IParametersValidator<CourseSummaryParameters>
{
    private readonly ClaimsPrincipal _user;

    public CourseSummaryParametersValidator(IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor.HttpContext!.User;
    }

    public Task<ValidationResult> ValidateAsync(CourseSummaryParameters parameters)
    {
        var studentClaimValidation = _user.ValidateParsableClaim<int>(CustomClaimTypes.StudentId);
        if (!studentClaimValidation.Result.IsValid)
        {
            return Task.FromResult(studentClaimValidation.Result);
        }

        if (parameters.StudentId != studentClaimValidation.ClaimValue)
        {
            return Task.FromResult(new ValidationResult(nameof(parameters.StudentId), 
                "You profile`s student id does not matches provided"));
        }

        return Task.FromResult(new ValidationResult());
    }
}