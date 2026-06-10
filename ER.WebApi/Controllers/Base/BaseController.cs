namespace ER.WebApi.Controllers.Base;

/// <summary>
/// Base controller providing current-user helpers and <see cref="Result{T}"/> to HTTP result mapping.
/// </summary>
public class BaseController : ControllerBase
{
    protected readonly ICurrentUserService CurrentUserService;
    protected readonly ILogger Logger;

    public BaseController(ICurrentUserService currentUserService, ILogger logger)
    {
        CurrentUserService = currentUserService;
        Logger = logger;
    }

    protected virtual (Guid TenantId, Guid EmployeeId) GetCurrentUserIds()
    {
        return (CurrentUserService.TenantId ?? Guid.Empty, CurrentUserService.EmployeeId ?? Guid.Empty);
    }

    protected LogContext CreateLogContext([CallerMemberName] string method = "")
        => new(GetType().Name, method);

    /// <summary>
    /// Maps a failed service result to the appropriate ASP.NET action result.
    /// </summary>
    protected ActionResult FromResult<T>(Result<T> result, Func<T, ActionResult> onSuccess)
    {
        if (result.Success)
        {
            return onSuccess(result.Data!);
        }

        if (!result.Validation.IsValid)
        {
            return BadRequest(result.Validation);
        }

        var primaryError = result.Error.FirstOrDefault();

        if (primaryError is null)
        {
            return BadRequest(new { message = "Request failed." });
        }

        return primaryError.ErrorType switch
        {
            ErrorType.Exception => StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." }),
            ErrorType.Service when IsForbidden(primaryError) => StatusCode(StatusCodes.Status403Forbidden, new { message = result.Error }),
            ErrorType.Service => BadRequest(new { message = result.Error }),
            _ => BadRequest(new { message = result.Error })
        };
    }

    /// <summary>
    /// Returns the HTTP status code that <see cref="FromResult{T}"/> would use for a failed result.
    /// </summary>
    protected int GetFailureStatusCode<T>(Result<T> result)
    {
        if (result.Success)
        {
            return StatusCodes.Status200OK;
        }

        if (!result.Validation.IsValid)
        {
            return StatusCodes.Status400BadRequest;
        }

        var primaryError = result.Error.FirstOrDefault();

        if (primaryError is null)
        {
            return StatusCodes.Status400BadRequest;
        }

        return primaryError.ErrorType switch
        {
            ErrorType.Exception => StatusCodes.Status500InternalServerError,
            ErrorType.Service when IsForbidden(primaryError) => StatusCodes.Status403Forbidden,
            ErrorType.Service => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status400BadRequest
        };
    }

    private static bool IsForbidden(Error error) =>
        error.ErrorMessage.Contains("not a manager", StringComparison.OrdinalIgnoreCase);
}
