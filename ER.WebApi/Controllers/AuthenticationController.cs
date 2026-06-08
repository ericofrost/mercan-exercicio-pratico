using ER.Application.Authentication;
using ER.Application.Interfaces.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ER.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
[AllowAnonymous]
public class AuthenticationController(IAuthenticationService authenticationService, IEmployeeRegistrationService registrationService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await authenticationService.LoginAsync(request, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : Unauthorized(new { message = result.Error });
    }

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
