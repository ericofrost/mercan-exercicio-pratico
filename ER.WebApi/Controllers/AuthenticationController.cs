namespace ER.WebApi.Controllers;

/// <summary>
/// Exposes anonymous authentication endpoints for login and employee registration.
/// </summary>
[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthenticationController(IAuthenticationService authenticationService, IEmployeeRegistrationService registrationService, ILogger<AuthenticationController> logger) : ControllerBase
{
    private const string InvalidCredentialsMessage = "Invalid credentials.";

    /// <summary>
    /// Authenticates an employee within a tenant and returns a JWT access token.
    /// </summary>
    /// <param name="request">The login request containing tenant, email, and password.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// <see cref="OkObjectResult"/> with a <see cref="LoginResponse"/> when authentication succeeds;
    /// otherwise <see cref="UnauthorizedObjectResult"/> with a generic error message.
    /// </returns>
    [HttpPost("login")]
    [EnableRateLimiting(LoginRateLimitSettings.PolicyName)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var ctx = LogContext.For<AuthenticationController>();
        
        ApiLogs.OperationStarted(logger, ctx, request.TenantId);

        var result = await authenticationService.LoginAsync(request, cancellationToken);

        if (!result.Success)
        {
            var statusCode = GetLoginFailureStatusCode(result);
            
            ApiLogs.OperationRejected(logger, ctx, request.TenantId, "Login request rejected", statusCode);
            
            return MapLoginFailure(result);
        }

        ApiLogs.OperationCompleted(logger, ctx, request.TenantId);

        return Ok(result.Data);
    }

    /// <summary>
    /// Registers a new employee and linked identity user within a tenant.
    /// </summary>
    /// <param name="request">The registration request containing tenant, profile, and password data.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// <see cref="CreatedAtActionResult"/> with a <see cref="RegisterEmployeeResult"/> when registration succeeds;
    /// otherwise <see cref="BadRequestObjectResult"/> with the validation or identity error message.
    /// </returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterEmployeeResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterEmployeeRequest request, CancellationToken cancellationToken)
    {
        var ctx = LogContext.For<AuthenticationController>();
        
        ApiLogs.OperationStarted(logger, ctx, request.TenantId);

        var result = await registrationService.RegisterAsync(request, cancellationToken);

        if (!result.Success)
        {
            var statusCode = GetRegisterFailureStatusCode(result);
            
            ApiLogs.OperationRejected(logger, ctx, request.TenantId, "Registration request rejected", statusCode);
            
            return MapRegisterFailure(result);
        }

        ApiLogs.OperationCompleted(logger, ctx, request.TenantId, result.Data!.EmployeeId, result.Data.UserId, request.Role);

        return CreatedAtAction(nameof(Login), result.Data);
    }

    private static int GetLoginFailureStatusCode(Result<LoginResponse> result)
    {
        if (!result.Validation.IsValid)
        {
            return StatusCodes.Status400BadRequest;
        }

        if (result.Error.Any(e => e.ErrorType == ErrorType.Exception))
        {
            return StatusCodes.Status500InternalServerError;
        }

        return StatusCodes.Status401Unauthorized;
    }

    private static int GetRegisterFailureStatusCode(Result<RegisterEmployeeResult> result)
    {
        if (!result.Validation.IsValid)
        {
            return StatusCodes.Status400BadRequest;
        }

        if (result.Error.Any(e => e.ErrorType == ErrorType.Exception))
        {
            return StatusCodes.Status500InternalServerError;
        }

        return StatusCodes.Status400BadRequest;
    }

    private IActionResult MapLoginFailure(Result<LoginResponse> result)
    {
        if (!result.Validation.IsValid)
        {
            return BadRequest(result.Validation);
        }

        if (result.Error.Any(e => e.ErrorType == ErrorType.Exception))
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = OperationMessages.UnexpectedError });
        }

        return Unauthorized(new { message = InvalidCredentialsMessage });
    }

    private IActionResult MapRegisterFailure(Result<RegisterEmployeeResult> result)
    {
        if (!result.Validation.IsValid)
        {
            return BadRequest(result.Validation);
        }

        if (result.Error.Any(e => e.ErrorType == ErrorType.Exception))
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = OperationMessages.UnexpectedError });
        }

        var message = result.Error.FirstOrDefault()?.ErrorMessage ?? "Registration failed.";
        
        return BadRequest(new { message });
    }
}
