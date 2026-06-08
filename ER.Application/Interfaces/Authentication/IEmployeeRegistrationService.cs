using ER.Application.Authentication;
using ER.Application.Common;

namespace ER.Application.Interfaces.Authentication;

/// <summary>
/// Application service contract for registering employees and their linked identity users.
/// </summary>
public interface IEmployeeRegistrationService
{
    /// <summary>
    /// Registers a new employee and identity user within the specified tenant.
    /// </summary>
    /// <param name="request">The registration request containing tenant, profile, and password data.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// A successful <see cref="Result{T}"/> when registration completes; otherwise a failed result describing the validation or identity error.
    /// </returns>
    Task<Result<RegisterEmployeeResult>> RegisterAsync(RegisterEmployeeRequest request, CancellationToken cancellationToken = default);
}
