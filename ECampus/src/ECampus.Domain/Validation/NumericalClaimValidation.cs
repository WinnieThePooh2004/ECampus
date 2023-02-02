using System.Security.Claims;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Validation;

public static class NumericalClaimValidation
{
    public static (ValidationResult Result, int? ClaimValue) ValidateNumericalClaim(this ClaimsPrincipal user, string claimName)
    {
        var claim = user.FindFirst(claimName);
        if (claim is null)
        {
            return (new ValidationResult(nameof(user), $"User must have claim '{claimName}'"), null);
        }

        if (!int.TryParse(claim.Value, out var claimValue))
        {
            return (new ValidationResult(nameof(user), $"Claim '{claimName}' must be a number, not '{claim.Value}'"), null);
        }

        return (new ValidationResult(), claimValue);
    }
}