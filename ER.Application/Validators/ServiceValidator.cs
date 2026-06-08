namespace ER.Application.Validators;

/// <summary>
/// Base FluentValidation adapter that populates a <see cref="Result{T}"/> with validation outcomes.
/// </summary>
/// <typeparam name="T">The request type to validate.</typeparam>
/// <typeparam name="TResult">The result payload type associated with the operation.</typeparam>
public abstract class ServiceValidator<T,TResult> : AbstractValidator<T> , IServiceValidator<T,TResult> where T : class where TResult : class
{
    /// <inheritdoc />
    public virtual async Task<bool> SetValidationResultAsync(Result<TResult> result, T data, CancellationToken cancellationToken = default)    {
        var validationResult = await this.ValidateAsync(data, cancellationToken);
        
        result.Validation = validationResult;

        if (!validationResult.IsValid)
        {
            result.Success = false;
        }
        
        return validationResult.IsValid;
    }
}