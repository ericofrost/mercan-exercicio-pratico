using ER.Application.Common;

namespace ER.Application.Interfaces.Validators;

public interface IServiceValidator<in T,TResult> where T : class where TResult : class
{
    Task<bool> SetValidationResultAsync(Result<TResult> result, T data, CancellationToken cancellationToken = default);
}