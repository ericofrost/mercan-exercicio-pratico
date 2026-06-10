namespace ER.Application.Services.Authentication;

/// <summary>
/// Validates tenant-scoped credentials through ASP.NET Identity and issues JWT access tokens for authenticated employees.
/// </summary>
public class AuthenticationService(UserManager<ApplicationUser> userManager, ITokenGeneratorService tokenGeneratorService, IServiceValidator<LoginRequest,LoginResponse>  loginRequestValidator, IOptions<JwtSettings> configuration, ILogger<AuthenticationService> logger) : IAuthenticationService
{
    private const string InvalidCredentialsMessage = "Invalid credentials.";

    /// <inheritdoc />
    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var ctx = LogContext.For<AuthenticationService>();
        var result = Result<LoginResponse>.Create();

        try
        {
            if (!await loginRequestValidator.SetValidationResultAsync(result, request, cancellationToken))
            {
                return result;
            }
        
            var user = await GetUserAsync(request, cancellationToken);

            await CheckUserPassword(user, request, result, cancellationToken);

            if (!result.Success) return result;
        
            var expiresAt = DateTime.UtcNow.AddMinutes(configuration.Value.ExpiryMinutes);

            var token = await tokenGeneratorService.GenerateTokenAsync(new GenerateTokenRequest(user!.Employee!.Id, user.TenantId, user.Email!, user.Employee.Role));
            
            CheckToken(token, user, request, result);
            
            if (!result.Success) return result;
        
            ApplicationLogs.OperationCompleted(logger, ctx, request.TenantId, user!.Employee!.Id);

            result.SetData(new LoginResponse(token!, expiresAt));
        }
        catch (Exception e)
        {
            ApplicationLogs.OperationFailedUnexpectedly(logger, e, ctx, request.TenantId, nameof(LoginAsync));
            
            result.SetError(e.Message, ErrorType.Exception);
        }

        return result;
    }

    private async Task CheckUserPassword(ApplicationUser? user, LoginRequest request, Result<LoginResponse> result, CancellationToken cancellationToken = default)
    {
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            var ctx = new LogContext(nameof(AuthenticationService), nameof(CheckUserPassword));
            
            ApplicationLogs.OperationRejected(logger, ctx, request.TenantId, InvalidCredentialsMessage);
            
            result.SetError(InvalidCredentialsMessage, ErrorType.Service);
        }
    }
    
    private void CheckToken(string? token, ApplicationUser user, LoginRequest request, Result<LoginResponse> result)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            var ctx = new LogContext(nameof(AuthenticationService), nameof(CheckToken));
            ApplicationLogs.OperationRejected(logger, ctx, request.TenantId, "Token generation empty", errorCodes: user.Employee!.Id.ToString());
            
            result.SetError(InvalidCredentialsMessage, ErrorType.Service);
        }
    }

    private async Task<ApplicationUser?> GetUserAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        return await userManager.Users.Include(u => u.Employee).FirstOrDefaultAsync(
            u => u.TenantId == request.TenantId && u.Email == request.Email && u.Employee!.IsActive, cancellationToken);
    }
}
