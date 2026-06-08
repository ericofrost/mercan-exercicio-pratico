namespace ER.Application.Interfaces.Authentication;

/// <summary>
/// Application service contract for employee authentication and JWT issuance.
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Validates tenant-scoped credentials and issues a JWT access token when authentication succeeds.
    /// </summary>
    /// <param name="request">The login request containing tenant, email, and password.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// A successful <see cref="Result{T}"/> when credentials are valid; otherwise a failed result with validation,
    /// service, or unexpected errors. Credential failures use a generic error message.
    /// </returns>
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
