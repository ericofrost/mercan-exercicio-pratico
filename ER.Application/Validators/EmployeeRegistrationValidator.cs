namespace ER.Application.Validators;

/// <summary>
/// Validates employee registration requests, including tenant existence and email uniqueness within the tenant.
/// </summary>
public class EmployeeRegistrationValidator : ServiceValidator<RegisterEmployeeRequest,RegisterEmployeeResult>
{
    private readonly IGenericRepository<Domain.Models.Employee> _employeeRepository;
    private readonly IGenericRepository<Tenant> _tenantRepository;
    
    /// <summary>
    /// Initializes a new instance of the registration validator with repository access from the unit of work.
    /// </summary>
    /// <param name="unitOfWork">The unit of work used to resolve tenant and employee repositories.</param>
    public EmployeeRegistrationValidator(IUnitOfWork unitOfWork)
    {
        _employeeRepository = unitOfWork.Repository<Employee>();
        _tenantRepository = unitOfWork.Repository<Tenant>();
        
        RuleFor(r => r.TenantId).NotNull().NotEmpty().MustAsync(TenantExistsAsync).WithMessage("Tenant not found or inactive.");
        RuleFor(r => r.Email).NotNull().NotEmpty();
        RuleFor(r => r).NotNull().MustAsync(EmailTakenAsync).WithMessage("Email already registered for this tenant.");
        RuleFor(r => r.Name).NotNull().NotEmpty();
        RuleFor(r => r.Password).NotNull().NotEmpty();
        RuleFor(r => r.Role).NotNull().NotEmpty();
    }

    private async Task<bool> TenantExistsAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        return await _tenantRepository.ExistsWithFilterAsync(t => t.Id == tenantId && t.IsActive, cancellationToken);
    }
    
    private async Task<bool> EmailTakenAsync(RegisterEmployeeRequest request, CancellationToken cancellationToken)
    {
        return await _employeeRepository.ExistsWithFilterAsync(e => e.TenantId == request.TenantId && e.Email == request.Email, cancellationToken);
    }
}