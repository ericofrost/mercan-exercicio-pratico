namespace ER.Application.Common;

/// <summary>
/// Common failure and validation surface shared by <see cref="Result{T}"/> and paginated results.
/// </summary>
public interface IOperationResult
{
    /// <summary>
    /// Gets a value indicating whether the operation completed successfully.
    /// </summary>
    bool IsSuccessful { get; }

    /// <summary>
    /// Gets the FluentValidation outcome for the operation.
    /// </summary>
    ValidationResult Validation { get; }

    /// <summary>
    /// Gets application errors populated when the operation fails.
    /// </summary>
    IReadOnlyList<Error> Errors { get; }
}
