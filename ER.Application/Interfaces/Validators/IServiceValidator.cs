namespace ER.Application.Interfaces.Validators;

/// <summary>
/// Validates request data and applies FluentValidation results to a shared <see cref="Result{T}"/>.
/// </summary>
/// <typeparam name="T">The request type to validate.</typeparam>
/// <typeparam name="TResult">The result payload type associated with the operation.</typeparam>
/// <typeparam name="TModel">Model for comparison</typeparam>
public interface IServiceValidator<in T, TResult> where T : class
{
    /// <summary>
    /// Runs validation for the supplied request and updates the result with any validation failures.
    /// </summary>
    /// <param name="result">The operation result to populate with validation errors.</param>
    /// <param name="data">The request data to validate.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when validation succeeds; otherwise <c>false</c>.</returns>
    Task<bool> SetValidationResultAsync(Result<TResult> result, T data, CancellationToken cancellationToken = default);
}