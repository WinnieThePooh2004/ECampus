using ECampus.Shared.Validation;

namespace ECampus.Shared.Extensions;

public static class ValidationResultExtensions
{
    public static void AddRange(this ValidationResult result, IEnumerable<ValidationError> errors)
    {
        foreach (var error in errors)
        {
            result.AddError(error);
        }
    }

    public static void MergeResults(this ValidationResult result, ValidationResult addFrom)
    {
        result.AddRange(addFrom);
    }
}