using ER.Application.Common;
using ER.Application.Interfaces.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace ER.Application.Validators;

public abstract class ServiceValidator<T,TResult> : AbstractValidator<T> , IServiceValidator<T,TResult> where T : class where TResult : class
{
    public virtual async Task<bool> SetValidationResultAsync(Result<TResult> result, T data, CancellationToken cancellationToken = default)
    {
        var validationResult = await this.ValidateAsync(data, cancellationToken);
        
        result.Validation = validationResult;
        
        return validationResult.IsValid;
    }
}