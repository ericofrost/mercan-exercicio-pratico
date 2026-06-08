using ER.Application.Authentication;
using ER.Application.Common;
using ER.Application.Interfaces.Authentication;
using ER.Domain.Configuration;
using ER.Domain.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ER.Application.Services.Authentication;

/// <summary>
/// Validates tenant-scoped credentials through ASP.NET Identity and issues JWT access tokens for authenticated employees.
/// </summary>
public class AuthenticationService(UserManager<ApplicationUser> userManager, ITokenGeneratorService tokenGeneratorService, IOptions<JwtSettings> configuration) : IAuthenticationService
{
    private const string InvalidCredentialsMessage = "Invalid credentials.";

    /// <inheritdoc />
    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.Users.Include(u => u.Employee).FirstOrDefaultAsync(
            u => u.TenantId == request.TenantId && u.Email == request.Email && u.Employee!.IsActive, cancellationToken);

        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result<LoginResponse>.Failure(InvalidCredentialsMessage);
        }

        var expiresAt = DateTime.UtcNow.AddMinutes(configuration.Value.ExpiryMinutes);

        var token = await tokenGeneratorService.GenerateTokenAsync(new GenerateTokenRequest(user.Employee!.Id, user.TenantId, user.Email!, user.Employee.Role));

        return string.IsNullOrEmpty(token) ? Result<LoginResponse>.Failure(InvalidCredentialsMessage) : Result<LoginResponse>.Success(new LoginResponse(token, expiresAt));
    }
}
