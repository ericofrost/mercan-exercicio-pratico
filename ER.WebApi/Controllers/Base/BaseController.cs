namespace ER.WebApi.Controllers.Base;

/// <summary>
/// Base controller providing current-user helpers and operation result to HTTP result mapping.
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
        => (CurrentUserService.TenantId ?? Guid.Empty, CurrentUserService.EmployeeId ?? Guid.Empty);

    protected LogContext CreateLogContext([CallerMemberName] string method = "") => new(GetType().Name, method);

    /// <summary>
    /// Maps a <see cref="Result{T}"/> to the appropriate ASP.NET action result.
    /// </summary>
    protected ActionResult FromResult<T>(Result<T> result, Func<T, ActionResult> onSuccess)
        => FromOperationResult(result, () => onSuccess(result.Data!));

    /// <summary>
    /// Maps a <see cref="PagedResult{T}"/> to the appropriate ASP.NET action result.
    /// </summary>
    protected ActionResult FromPagedResult<T>(PagedResult<T> result, Func<PagedResult<T>, ActionResult> onSuccess)
        => FromOperationResult(result, () => onSuccess(result));

    /// <summary>
    /// Returns the HTTP status code that failure mapping would use for a <see cref="Result{T}"/>.
    /// </summary>
    protected int GetFailureStatusCode<T>(Result<T> result) => GetFailureStatusCode((IOperationResult)result);

    /// <summary>
    /// Returns the HTTP status code that failure mapping would use for a <see cref="PagedResult{T}"/>.
    /// </summary>
    protected int GetFailureStatusCode<T>(PagedResult<T> result) => GetFailureStatusCode((IOperationResult)result);

    private ActionResult FromOperationResult(IOperationResult result, Func<ActionResult> onSuccess)
    {
        if (result.IsSuccessful)
        {
            return onSuccess();
        }

        return MapFailureToActionResult(result);
    }

    private static int GetFailureStatusCode(IOperationResult result)
    {
        if (result.IsSuccessful)
        {
            return StatusCodes.Status200OK;
        }

        if (!result.Validation.IsValid)
        {
            return StatusCodes.Status400BadRequest;
        }

        var primaryError = result.Errors.FirstOrDefault();

        if (primaryError is null)
        {
            return StatusCodes.Status400BadRequest;
        }

        return primaryError.ErrorType switch
        {
            ErrorType.Exception => StatusCodes.Status500InternalServerError,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Service when IsForbidden(primaryError) => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status400BadRequest
        };
    }

    private ActionResult MapFailureToActionResult(IOperationResult result)
    {
        if (!result.Validation.IsValid)
        {
            return BadRequest(result.Validation);
        }

        var primaryError = result.Errors.FirstOrDefault();

        if (primaryError is null)
        {
            return BadRequest(new { message = "Request failed." });
        }

        return primaryError.ErrorType switch
        {
            ErrorType.Exception => StatusCode(StatusCodes.Status500InternalServerError, new { message = OperationMessages.UnexpectedError }),
            ErrorType.NotFound => NotFound(new { message = primaryError.ErrorMessage }),
            ErrorType.Service when IsForbidden(primaryError) => StatusCode(StatusCodes.Status403Forbidden, new { message = primaryError.ErrorMessage }),
            ErrorType.Service => BadRequest(new { message = primaryError.ErrorMessage }),
            _ => BadRequest(new { message = primaryError.ErrorMessage })
        };
    }

    private static bool IsForbidden(Error error) => error.ErrorMessage.Contains("not a manager", StringComparison.OrdinalIgnoreCase);
}
