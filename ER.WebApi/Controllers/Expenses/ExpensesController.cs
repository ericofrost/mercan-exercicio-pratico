namespace ER.WebApi.Controllers.Expenses;

/// <summary>
/// Expense endpoints for managers to review and manage tenant expenses.
/// </summary>
[ApiController]
[Route("api/expenses")]
[Authorize]
public class ExpensesController(IExpensesService expensesService, ICurrentUserService currentUserService, IMapper mapper, ILogger<ExpensesController> logger) : BaseController(currentUserService, logger)
{
    /// <summary>
    /// Returns all pending expenses for the authenticated manager's tenant.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// <see cref="OkObjectResult"/> with pending expenses when the request succeeds;
    /// otherwise an error response mapped from the service result.
    /// </returns>
    [HttpGet("pending")]
    [ProducesResponseType(typeof(GetPendingExpensesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetPendingExpensesResponse>> GetPendingExpensesAsync(CancellationToken cancellationToken)
    {
        var ctx = CreateLogContext();
        var user = GetCurrentUserIds();

        ApiLogs.OperationStarted(logger, ctx, user.TenantId, user.EmployeeId);

        var result = await expensesService.GetAllExpensesByManagerTenant(cancellationToken);

        if (!result.Success)
        {
            ApiLogs.OperationRejected(logger, ctx, user.TenantId, "Request failed", GetFailureStatusCode(result));
            
            return FromResult(result, _ => Ok());
        }

        var response = mapper.Map<GetPendingExpensesResponse>(result.Data);

        ApiLogs.OperationCompleted(logger, ctx, user.TenantId, detail: $"count={result.Data!.Count}");

        return response.Expenses.Any() ? Ok(response) : NotFound();
    }

    /// <summary>
    /// Submits a new expense with initial status Pending.
    /// </summary>
    /// <param name="request">The expense being submitted.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// <see cref="OkResult"/> when submission succeeds; otherwise an error response mapped from the service result.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SubmitExpenseAsync([FromBody] SubmitExpenseRequest request, CancellationToken cancellationToken)
    {
        var ctx = CreateLogContext();
        var user = GetCurrentUserIds();

        ApiLogs.OperationStarted(logger, ctx, user.TenantId, user.EmployeeId);

        var dto = mapper.Map<SubmitExpenseRequestDto>(request);
        
        var result = await expensesService.SubmitExpenseAsync(dto, cancellationToken);

        if (!result.Success)
        {
            ApiLogs.OperationRejected(logger, ctx, user.TenantId, "Request failed", GetFailureStatusCode(result));
            
            return FromResult(result, _ => Ok());
        }

        ApiLogs.OperationCompleted(logger, ctx, user.TenantId, user.EmployeeId);

        return Ok();
    }

    /// <summary>
    /// Approves an expense with initial status Pending.
    /// </summary>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <param name="id">The Expense ID</param>
    /// <returns>
    /// <see cref="OkResult"/> when submission succeeds; otherwise an error response mapped from the service result.
    /// </returns>
    [HttpPost("{id:guid}/approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> ApproveExpenseAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var ctx = CreateLogContext();
        var user = GetCurrentUserIds();

        ApiLogs.OperationStarted(logger, ctx, user.TenantId, user.EmployeeId);
        
        var result = await expensesService.ApproveExpenseAsync(new ChangeExpenseStatusDto(id, ExpenseStatus.Approved), cancellationToken);

        if (!result.Success)
        {
            ApiLogs.OperationRejected(logger, ctx, user.TenantId, "Request failed", GetFailureStatusCode(result));
            
            return FromResult(result, _ => Ok());
        }

        ApiLogs.OperationCompleted(logger, ctx, user.TenantId, user.EmployeeId);

        return Ok();
    }
    
    /// <summary>
    /// Approves an expense with initial status Pending.
    /// </summary>
    /// <param name="request">The expense being submitted.</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <param name="id">The Expense ID</param>
    /// <returns>
    /// <see cref="OkResult"/> when submission succeeds; otherwise an error response mapped from the service result.
    /// </returns>
    [HttpPost("{id:guid}/reject")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> RejectExpenseAsync([FromRoute] Guid id, [FromBody] RejectExpenseRequest request, CancellationToken cancellationToken)
    {
        var ctx = CreateLogContext();
        var user = GetCurrentUserIds();

        ApiLogs.OperationStarted(logger, ctx, user.TenantId, user.EmployeeId);
        
        var result = await expensesService.RejectExpenseAsync(new ChangeExpenseStatusDto(id, ExpenseStatus.Rejected, request.RejectReason), cancellationToken);

        if (!result.Success)
        {
            ApiLogs.OperationRejected(logger, ctx, user.TenantId, "Request failed", GetFailureStatusCode(result));
            
            return FromResult(result, _ => Ok());
        }

        ApiLogs.OperationCompleted(logger, ctx, user.TenantId, user.EmployeeId);

        return Ok();
    }

    /// <summary>
    /// Returns all pending expenses for the authenticated user's tenant.
    /// </summary>
    /// <param name="request">Pagination request</param>
    /// <param name="cancellationToken">Token used to cancel the operation.</param>
    /// <returns>
    /// <see cref="OkObjectResult"/> with pending expenses when the request succeeds;
    /// otherwise an error response mapped from the service result.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Expense>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Expense>>> GetExpensesAsync([FromQuery] GetPaginatedExpensesRequest request, CancellationToken cancellationToken)
    {
        var ctx = CreateLogContext();
        var user = GetCurrentUserIds();

        ApiLogs.OperationStarted(logger, ctx, user.TenantId, user.EmployeeId);

        var dto = mapper.Map<PaginationRequestDto>(request);
        
        var result = await expensesService.GetAllExpensesByEmployeeTenant(dto, cancellationToken);

        if (!result.Success)
        {
            ApiLogs.OperationRejected(logger, ctx, user.TenantId, "Request failed", GetFailureStatusCode(result));
            
            return FromPagedResult(result, _ => Ok());
        }

        var response = mapper.Map<IEnumerable<Expense>>(result.Items);

        ApiLogs.OperationCompleted(logger, ctx, user.TenantId, detail: $"count={result.Items.Count}");

        return result.Items.Count > 0 ? Ok(response) : NotFound();
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetPendingExpensesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<DetailedExpense>> GetExpenseDetailsAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var ctx = CreateLogContext();
        var user = GetCurrentUserIds();

        ApiLogs.OperationStarted(logger, ctx, user.TenantId, user.EmployeeId);
        
        var result = await expensesService.GetDetailedExpenseById(id, cancellationToken);
        
        if (!result.Success)
        {
            ApiLogs.OperationRejected(logger, ctx, user.TenantId, "Request failed", GetFailureStatusCode(result));
            
            return FromResult(result, _ => Ok());
        }

        var response = mapper.Map<DetailedExpense>(result.Data);

        ApiLogs.OperationCompleted(logger, ctx, user.TenantId, detail: $"expenseId={id}");

        return response != null? Ok(response) : NotFound();
    }
}
