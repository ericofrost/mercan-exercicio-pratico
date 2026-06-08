using ER.Application.Authentication;
using ER.Application.Interfaces.Repositories;
using ER.Domain.Models;
using FluentValidation;

namespace ER.Application.Validators;

public class EmployeeRegistrationValidator : ServiceValidator<RegisterEmployeeRequest,RegisterEmployeeResult>
{
    private readonly IGenericRepository<Domain.Models.Employee> _employeeRepository;
    private readonly IGenericRepository<Tenant> _tenantRepository;
    
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