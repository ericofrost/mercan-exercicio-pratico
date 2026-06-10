namespace ER.Application.Validators;

/// <summary>
/// Base FluentValidation adapter that populates a <see cref="Result{T}"/> with validation outcomes.
/// </summary>
/// <typeparam name="T">The request type to validate.</typeparam>
/// <typeparam name="TResult">The result payload type associated with the operation.</typeparam>
/// <typeparam name="TModel">Database Model for comparison </typeparam>
public abstract class ServiceValidator<T, TResult>(IUnitOfWork unitOfWork) : AbstractValidator<T>, IServiceValidator<T, TResult>
    where T : class
{
    protected readonly IGenericRepository<Employee> EmployeeRepository = unitOfWork.Repository<Employee>();
    protected readonly IGenericRepository<Tenant> TenantRepository = unitOfWork.Repository<Tenant>();

    /// <inheritdoc />
    public virtual async Task<bool> SetValidationResultAsync(Result<TResult> result, T data, CancellationToken cancellationToken = default)    
    {
        var validationResult = await this.ValidateAsync(data, cancellationToken);
        
        result.Validation = validationResult;

        if (!validationResult.IsValid)
        {
            result.Success = false;
        }
        
        return validationResult.IsValid;
    }

    protected virtual async Task<bool> TenantExistsAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        return await TenantRepository.ExistsWithFilterAsync(t => t.Id == tenantId && t.IsActive, cancellationToken);
    }
    
    protected virtual async Task<bool> EmailTakenAsync(RegisterEmployeeRequest request, CancellationToken cancellationToken)
    {
        return await EmployeeRepository.ExistsWithFilterAsync(e => e.TenantId == request.TenantId && e.Email == request.Email, cancellationToken);
    }
}