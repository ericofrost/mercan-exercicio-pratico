using ER.Application.Authentication;
using ER.Application.Common;
using ER.Application.Interfaces.Authentication;
using ER.Application.Interfaces.Repositories;
using ER.Domain.Models;
using ER.Domain.Shared;
using ER.Domain.Specifications;
using Microsoft.AspNetCore.Identity;

namespace ER.Application.Services.Employee;

/// <summary>
/// Registers employees and linked identity users atomically within a tenant using a unit-of-work transaction.
/// </summary>
public class EmployeeRegistrationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : IEmployeeRegistrationService
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
        var tenantExists = await _tenantRepository.ExistsWithFilterAsync(t => t.Id == request.TenantId && t.IsActive, cancellationToken);

        if (!tenantExists)
        {
            return Result<RegisterEmployeeResult>.Failure("Tenant not found or inactive.");
        }

        var emailTaken = await _employeeRepository.ExistsWithFilterAsync(e => e.TenantId == request.TenantId && e.Email == request.Email, cancellationToken);

        if (emailTaken)
        {
            return Result<RegisterEmployeeResult>.Failure("Email already registered for this tenant.");
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

                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));

                return Result<RegisterEmployeeResult>.Failure($"User creation failed: {errors}");
            }

            await unitOfWork.CommitTransactionAsync();

            return Result<RegisterEmployeeResult>.Success(new RegisterEmployeeResult(employee.Id, user.Id));
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
