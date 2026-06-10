namespace ER.Application.Interfaces.Validators;

public interface IServiceValidatorModelComparer<in T, in TModel,TResult> : IServiceValidator<T, TResult>  where T : class
{
    /// <summary>
    /// Runs validation for the supplied request and updates the result with any validation failures.
    /// </summary>
    /// <param name="result">The operation result to populate with validation errors.</param>
    /// <param name="data">The request data to validate.</param>
    /// <param name="dataCompare">Model to compare against. Avoids repeated database operations</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns><c>true</c> when validation succeeds; otherwise <c>false</c>.</returns>
    Task<bool> SetValidationResulWithComparerAsync(Result<TResult> result, T data, TModel dataCompare, CancellationToken cancellationToken = default);
}