using ER.Application.Authentication;
using ER.Application.Common;

namespace ER.Application.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
}
