using ER.Application.Authentication;
using ER.Application.Common;
using ER.Application.Interfaces.Authentication;
using ER.Application.Interfaces.Repositories;
using ER.Application.Interfaces.Validators;
using ER.Application.Logging;
using ER.Domain.Models;
using ER.Domain.Shared;
using ER.Domain.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ER.Application.Services.Employee;

/// <summary>
/// Registers employees and linked identity users atomically within a tenant using a unit-of-work transaction.
/// </summary>
public class EmployeeRegistrationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IServiceValidator<RegisterEmployeeRequest,RegisterEmployeeResult> validator, ILogger<EmployeeRegistrationService> logger) : IEmployeeRegistrationService
{
    private readonly IGenericRepository<Domain.Models.Employee> _employeeRepository = unitOfWork.Repository<Domain.Models.Employee>();
    private readonly IGenericRepository<Tenant> _tenantRepository = unitOfWork.Repository<Tenant>();

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// Thrown by the unit of work when transaction state is invalid.
    /// </exception>
    /// <exception cref="Exception">
    /// Rethrows unexpected persistence or identity exceptions after rolling back the active transaction.
    /// </exception>
    public async Task<Result<RegisterEmployeeResult>> RegisterAsync(RegisterEmployeeRequest request, CancellationToken cancellationToken = default)
    {
        var result = Result<RegisterEmployeeResult>.Create();

        if (!await validator.SetValidationResultAsync(result, request, cancellationToken))
        {
            return result;
        }

        await unitOfWork.BeginTransactionAsync();

        try
        {
            var employee = Domain.Models.Employee.Create(new EmployeeSpecification(request.TenantId, request.Name, request.Email, request.Role, true));

            await _employeeRepository.AddAsync(employee, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var user = ApplicationUser.Create(new ApplicationUserSpecification(request.TenantId, employee.Id, employee.Email, true));

            var createResult = await userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                await unitOfWork.RollbackTransactionAsync();

                var errorCodes = string.Join(", ", createResult.Errors.Select(e => e.Code));
                
                EmployeeRegistrationServiceLogs.IdentityUserCreationFailed(logger, request.TenantId, errorCodes);
                
                result.SetError("Email already registered for this tenant.", ErrorType.Service);
                
                return result;
            }

            await unitOfWork.CommitTransactionAsync();

            EmployeeRegistrationServiceLogs.RegistrationSucceeded(logger, request.TenantId, employee.Id, user.Id, request.Role);
            
            result.SetData(new RegisterEmployeeResult(employee.Id, user.Id));
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            
            EmployeeRegistrationServiceLogs.RegistrationFailedUnexpectedly(logger, ex, request.TenantId);
            
            result.SetError(ex.Message, ErrorType.Exception);
        }
        
        return result;
    }
}
