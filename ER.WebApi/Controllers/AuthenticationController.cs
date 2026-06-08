using ER.Application.Authentication;
using ER.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ER.WebApi.Controllers;

/// <summary>
/// Exposes anonymous authentication endpoints for login and employee registration.
/// </summary>
[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthenticationController(IAuthenticationService authenticationService, IEmployeeRegistrationService registrationService) : ControllerBase
{
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
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.LoginAsync(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : Unauthorized(new { message = result.Error });
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
    public async Task<IActionResult> Register([FromBody] RegisterEmployeeRequest request, CancellationToken cancellationToken)
    {
        var result = await registrationService.RegisterAsync(request, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { message = result.Error });
        }

        return CreatedAtAction(nameof(Login), result.Value);
    }
}
