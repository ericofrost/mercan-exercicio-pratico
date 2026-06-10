namespace ER.Application.Validators;

public abstract class ServiceValidatorModelComparer<T, TModel,TResult>(IUnitOfWork unitOfWork) : ServiceValidator<T, TResult>(unitOfWork), IServiceValidatorModelComparer<T, TModel,TResult> where T : class
{
    protected TModel Compare;
    
    public async Task<bool> SetValidationResulWithComparerAsync(Result<TResult> result, T data, TModel dataCompare, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dataCompare, nameof(TModel));
        
        Compare = dataCompare;
        
        return (await SetValidationResultAsync(result, data, cancellationToken));
    }
}