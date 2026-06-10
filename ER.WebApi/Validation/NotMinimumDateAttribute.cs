namespace ER.WebApi.Validation;

/// <summary>
/// Validates that a date value is not the type minimum (e.g. <see cref="DateOnly.MinValue"/>).
/// </summary>
public sealed class NotMinimumDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is DateOnly dateOnly && dateOnly == DateOnly.MinValue)
        {
            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be a valid date.");
        }

        if (value is DateTime dateTime && dateTime == DateTime.MinValue)
        {
            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be a valid date.");
        }

        return ValidationResult.Success;
    }
}
