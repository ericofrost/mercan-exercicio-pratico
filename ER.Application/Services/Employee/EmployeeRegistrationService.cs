namespace ER.Application.Services.Employee;

/// <summary>
/// Registers employees and linked identity users atomically within a tenant using a unit-of-work transaction.
/// Tenant and email checks run through <see cref="Validators.EmployeeRegistrationValidator"/> before the transaction starts.
/// </summary>
public class EmployeeRegistrationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IServiceValidator<RegisterEmployeeRequest,RegisterEmployeeResult> validator, ILogger<EmployeeRegistrationService> logger) : IEmployeeRegistrationService
{
    private readonly IGenericRepository<Domain.Models.Employee> _employeeRepository = unitOfWork.Repository<Domain.Models.Employee>();
    private readonly IGenericRepository<Tenant> _tenantRepository = unitOfWork.Repository<Tenant>();

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// Thrown by the unit of work when transaction state is invalid.
    /// </exception>
    public async Task<Result<RegisterEmployeeResult>> RegisterAsync(RegisterEmployeeRequest request, CancellationToken cancellationToken = default)
    {
        var ctx = LogContext.For<EmployeeRegistrationService>();
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
                
                ApplicationLogs.OperationRejected(logger, ctx, request.TenantId, "Identity user creation failed", errorCodes: errorCodes);
                
                result.SetError("User creation failed.", ErrorType.Service);
                
                return result;
            }

            await unitOfWork.CommitTransactionAsync();

            ApplicationLogs.OperationCompleted(logger, ctx, request.TenantId, employee.Id, user.Id, request.Role);
            
            result.SetData(new RegisterEmployeeResult(employee.Id, user.Id));
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            
            ApplicationLogs.OperationFailedUnexpectedly(logger, ex, ctx, request.TenantId, nameof(RegisterAsync));
            
            result.SetError(ex.Message, ErrorType.Exception);
        }
        
        return result;
    }
}
