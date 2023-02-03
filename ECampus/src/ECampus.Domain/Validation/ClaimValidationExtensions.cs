using System.Globalization;
using System.Security.Claims;
using ECampus.Shared.Validation;

namespace ECampus.Domain.Validation;

public static class ClaimValidationExtensions
{
    public static (ValidationResult Result, TClaimValue? ClaimValue) ValidateParsableClaim<TClaimValue>(
        this ClaimsPrincipal user, string claimName)
        where TClaimValue : struct, IParsable<TClaimValue>
    {
        var claim = user.FindFirst(claimName);
        if (claim is null)
        {
            return (new ValidationResult(nameof(user), $"User must have claim '{claimName}'"), null);
        }

        if (!TClaimValue.TryParse(claim.Value, CultureInfo.CurrentCulture, out var claimValue))
        {
            return (new ValidationResult(nameof(user), $"Claim '{claimName}' must be a number, not '{claim.Value}'"),
                null);
        }

        return (new ValidationResult(), claimValue);
    }

    public static (ValidationResult Result, TEnum? ClaimValue) ValidateEnumClaim<TEnum>(
        this ClaimsPrincipal user, string claimName)
        where TEnum : struct
    {
        var claim = user.FindFirst(claimName);
        if (claim is null)
        {
            return (new ValidationResult(nameof(user), $"User must have claim '{claimName}'"), null);
        }

        if (!Enum.TryParse<TEnum>(claim.Value, out var claimValue))
        {
            return (new ValidationResult(nameof(user), $"Claim '{claimName}' must be a number, not '{claim.Value}'"),
                null);
        }

        return (new ValidationResult(), claimValue);
    }
}