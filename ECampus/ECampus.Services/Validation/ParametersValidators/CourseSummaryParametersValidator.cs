using System.Security.Claims;
using ECampus.Domain.Auth;
using ECampus.Domain.Requests.Course;
using ECampus.Domain.Validation;
using ECampus.Services.Contracts.Validation;
using Microsoft.AspNetCore.Http;

namespace ECampus.Services.Validation.ParametersValidators;

public class CourseSummaryParametersValidator : IParametersValidator<CourseSummaryParameters>
{
    private readonly ClaimsPrincipal _user;

    public CourseSummaryParametersValidator(IHttpContextAccessor httpContextAccessor)
    {
        _user = httpContextAccessor.HttpContext!.User;
    }

    public Task<ValidationResult> ValidateAsync(CourseSummaryParameters parameters, CancellationToken token = default)
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