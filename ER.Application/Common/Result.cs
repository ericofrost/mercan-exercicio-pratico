using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace ER.Application.Common;

/// <summary>
/// Represents the outcome of an application operation with a typed payload.
/// </summary>
/// <typeparam name="T">The type of the successful result value.</typeparam>
public class Result<T> : IOperationResult
{
    /// <summary>
    /// Gets or sets the successful payload when <see cref="Success"/> is <c>true</c>.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the operation completed successfully.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets the collection of application errors populated when the operation fails.
    /// </summary>
    public List<Error> Error { get; private set; } = [];
    
    /// <summary>
    /// Gets or sets the FluentValidation result for request validation failures.
    /// </summary>
    public required ValidationResult Validation { get; set; }

    /// <summary>
    /// Creates a new successful result with default data and an empty validation result.
    /// </summary>
    /// <returns>An initialized <see cref="Result{T}"/> with <see cref="Success"/> set to <c>true</c>.</returns>
    public static Result<T> Create()
    {
        return new Result<T>
        {
            Data = default,
            Success = true,
            Validation = new ValidationResult()
        };
    }
    
    /// <summary>
    /// Assigns the successful payload for this result.
    /// </summary>
    /// <param name="data">The value to return to the caller on success.</param>
    public void SetData(T data)
    {
        Data = data;
    }
    
    /// <summary>
    /// Records an error, marks the result as failed, and appends an entry to <see cref="Error"/>.
    /// </summary>
    /// <param name="errorMessage">The human-readable error description.</param>
    /// <param name="errorType">The category of the error.</param>
    public void SetError(string errorMessage, ErrorType errorType)
    { 
        Error.Add(new Error(errorMessage, errorType));
        Success = false;
    }

    bool IOperationResult.IsSuccessful => Success;

    ValidationResult IOperationResult.Validation => Validation;

    IReadOnlyList<Error> IOperationResult.Errors => Error;
}