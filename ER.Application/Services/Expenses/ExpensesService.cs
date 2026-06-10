namespace ER.Application.Services.Expenses;

public class ExpensesService(
    IUnitOfWork unitOfWork,
    ILogger<ExpensesService> logger,
    IServiceValidator<SubmitExpenseRequestDto, bool> submitValidator,
    IPaginationValidator paginationValidator,
    IServiceValidatorModelComparer<ChangeExpenseStatusDto, Expense, bool> approvalValidator,
    ICurrentUserService currentUserService) : IExpensesService
{
    private readonly IGenericRepository<Expense> _expensesRepository = unitOfWork.Repository<Expense>();

    public async Task<ExpenseByTenantResult> GetAllExpensesByManagerTenant(CancellationToken cancellationToken = default)
    {
        var ctx = LogContext.For<ExpensesService>();
        var tenantId = currentUserService.TenantId!.Value;
        var employeeId = currentUserService.EmployeeId ?? Guid.Empty;

        ApplicationLogs.OperationStarted(logger, ctx, tenantId, employeeId);

        var result = ExpenseByTenantResult.Create();

        try
        {
            var expenses = await _expensesRepository.GetAllWithFiltersAsync(exp => exp.TenantId == tenantId && exp.Status == ExpenseStatus.Pending, cancellationToken, exp => exp.Employee);

            expenses.ToList().ForEach(x => result.Data!.Add(ExpenseDto.FromModel(x)));

            ApplicationLogs.OperationCompleted(logger, ctx, tenantId, employeeId, detail: $"count={result.Data!.Count}");
        }
        catch (Exception ex)
        {
            ApplicationLogs.OperationFailedUnexpectedly(logger, ex, ctx, tenantId, nameof(GetAllExpensesByManagerTenant));
            result.SetError(OperationMessages.UnexpectedError, ErrorType.Exception);
        }

        return result;
    }

    public async Task<PagedResult<ExpenseDto>> GetAllExpensesByEmployeeTenant(PaginationRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        var ctx = LogContext.For<ExpensesService>();
        var tenantId = currentUserService.TenantId!.Value;
        var employeeId = currentUserService.EmployeeId ?? Guid.Empty;

        ApplicationLogs.OperationStarted(logger, ctx, tenantId, employeeId);

        var result = PagedResult<ExpenseDto>.Create();

        try
        {
            if (!await paginationValidator.SetValidationResultAsync(result, requestDto, cancellationToken))
            {
                ApplicationLogs.OperationRejected(logger, ctx, tenantId, "Pagination validation failed", StatusCodes.Status400BadRequest);
                return result;
            }

            result = PagedResult<ExpenseDto>.FromPaginatedRequest(requestDto);

            var expenses = await _expensesRepository.GetPaginatedListAsync(
                exp => exp.TenantId == tenantId && exp.Status == ExpenseStatus.Pending,
                requestDto.OrderBy,
                requestDto.Order,
                requestDto.RowsPerPage,
                requestDto.CurrentPage,
                cancellationToken);

            if (expenses.TotalCount == 0)
            {
                result.SetError("No Expenses found.", ErrorType.NotFound);
                
                ApplicationLogs.OperationRejected(logger, ctx, tenantId, "No expenses found", StatusCodes.Status404NotFound);
                
                return result;
            }

            result = PagedResult<ExpenseDto>.FromRepository(expenses.TotalCount, expenses.Records.Select(ExpenseDto.FromModel), requestDto);

            ApplicationLogs.OperationCompleted(logger, ctx, tenantId, employeeId, detail: $"count={result.Items.Count}");
        }
        catch (Exception ex)
        {
            ApplicationLogs.OperationFailedUnexpectedly(logger, ex, ctx, tenantId, nameof(GetAllExpensesByEmployeeTenant));
            result.SetError(OperationMessages.UnexpectedError, ErrorType.Exception);
        }

        return result;
    }

    public async Task<Result<bool>> SubmitExpenseAsync(SubmitExpenseRequestDto expense, CancellationToken cancellationToken = default)
    {
        var ctx = LogContext.For<ExpensesService>();
        var tenantId = currentUserService.TenantId!.Value;
        var employeeId = currentUserService.EmployeeId ?? Guid.Empty;

        ApplicationLogs.OperationStarted(logger, ctx, tenantId, employeeId);

        var result = Result<bool>.Create();

        try
        {
            if (!await submitValidator.SetValidationResultAsync(result, expense, cancellationToken))
            {
                ApplicationLogs.OperationRejected(logger, ctx, tenantId, "Submit expense validation failed", StatusCodes.Status400BadRequest);
                return result;
            }

            await unitOfWork.BeginTransactionAsync();
            
            await _expensesRepository.AddAsync(expense.ToModel(), cancellationToken);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync();

            ApplicationLogs.OperationCompleted(logger, ctx, tenantId, employeeId);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            
            ApplicationLogs.OperationFailedUnexpectedly(logger, ex, ctx, tenantId, nameof(SubmitExpenseAsync));
            
            result.SetError(OperationMessages.UnexpectedError, ErrorType.Exception);
        }

        return result;
    }

    public async Task<Result<bool>> ApproveExpenseAsync(ChangeExpenseStatusDto approvalRequest, CancellationToken cancellationToken = default)
    {
        var ctx = LogContext.For<ExpensesService>();
        var tenantId = currentUserService.TenantId!.Value;
        var employeeId = currentUserService.EmployeeId ?? Guid.Empty;

        ApplicationLogs.OperationStarted(logger, ctx, tenantId, employeeId);

        var result = Result<bool>.Create();

        try
        {
            var expense = (await _expensesRepository.GetAllWithFiltersAsync(exp => exp.Id == approvalRequest.ExpenseId, cancellationToken, exp => exp.Tenant)) .FirstOrDefault();

            if (expense == null)
            {
                result.SetError("Expense not found.", ErrorType.NotFound);
                ApplicationLogs.OperationRejected(logger, ctx, tenantId, "Expense not found", StatusCodes.Status404NotFound, approvalRequest.ExpenseId.ToString());
                return result;
            }

            if (!await approvalValidator.SetValidationResulWithComparerAsync(result, approvalRequest, expense, cancellationToken))
            {
                ApplicationLogs.OperationRejected(logger, ctx, tenantId, "Approve expense validation failed", StatusCodes.Status400BadRequest, approvalRequest.ExpenseId.ToString());
                return result;
            }

            await unitOfWork.BeginTransactionAsync();

            var expenseMonth = expense.ExpenseDate;
            var totalExpenses = (await _expensesRepository.GetAllWithFiltersAsync(
                    exp => exp.EmployeeId == expense.EmployeeId
                           && exp.TenantId == expense.TenantId
                           && exp.Status == ExpenseStatus.Approved
                           && exp.Id != expense.Id
                           && exp.ExpenseDate.Year == expenseMonth.Year
                           && exp.ExpenseDate.Month == expenseMonth.Month,
                    cancellationToken))
                .Sum(e => e.Amount);

            if (!expense.ValidateApprovalAmount(totalExpenses))
            {
                await unitOfWork.RollbackTransactionAsync();

                result.SetError(
                    $"This approval is not possible. Reason: It exceeds the Monthly Expense Limit for the Tenant: {expense.TenantId} and Employee: {expense.EmployeeId}",
                    ErrorType.Service);

                ApplicationLogs.OperationRejected(logger, ctx, tenantId, "Monthly expense limit exceeded", StatusCodes.Status400BadRequest, approvalRequest.ExpenseId.ToString());
                return result;
            }

            expense.SetApprovalDetails(ExpenseStatus.Approved, employeeId);
            
            await _expensesRepository.UpdateAsync(expense, cancellationToken);
            
            await unitOfWork.CommitTransactionAsync();

            ApplicationLogs.OperationCompleted(logger, ctx, tenantId, employeeId, detail: $"expenseId={approvalRequest.ExpenseId}");
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            
            ApplicationLogs.OperationFailedUnexpectedly(logger, ex, ctx, tenantId, nameof(ApproveExpenseAsync));
            
            result.SetError(OperationMessages.UnexpectedError, ErrorType.Exception);
        }

        return result;
    }

    public async Task<Result<bool>> RejectExpenseAsync(ChangeExpenseStatusDto rejectRequest, CancellationToken cancellationToken = default)
    {
        var ctx = LogContext.For<ExpensesService>();
        var tenantId = currentUserService.TenantId!.Value;
        var employeeId = currentUserService.EmployeeId ?? Guid.Empty;

        ApplicationLogs.OperationStarted(logger, ctx, tenantId, employeeId);

        var result = Result<bool>.Create();

        try
        {
            var expense = (await _expensesRepository.GetAllWithFiltersAsync(exp => exp.Id == rejectRequest.ExpenseId, cancellationToken)) .FirstOrDefault();

            if (expense == null)
            {
                result.SetError("Expense not found.", ErrorType.NotFound);
                ApplicationLogs.OperationRejected(logger, ctx, tenantId, "Expense not found", StatusCodes.Status404NotFound, rejectRequest.ExpenseId.ToString());
                return result;
            }

            if (!await approvalValidator.SetValidationResulWithComparerAsync(result, rejectRequest, expense, cancellationToken))
            {
                ApplicationLogs.OperationRejected(logger, ctx, tenantId, "Reject expense validation failed", StatusCodes.Status400BadRequest, rejectRequest.ExpenseId.ToString());
                return result;
            }

            await unitOfWork.BeginTransactionAsync();
            
            expense.SetApprovalDetails(ExpenseStatus.Rejected, employeeId, rejectRequest.RejectReason);
            
            await _expensesRepository.UpdateAsync(expense, cancellationToken);
            await unitOfWork.CommitTransactionAsync();

            ApplicationLogs.OperationCompleted(logger, ctx, tenantId, employeeId, detail: $"expenseId={rejectRequest.ExpenseId}");
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            
            ApplicationLogs.OperationFailedUnexpectedly(logger, ex, ctx, tenantId, nameof(RejectExpenseAsync));
            
            result.SetError(OperationMessages.UnexpectedError, ErrorType.Exception);
        }

        return result;
    }

    public async Task<DetailedExpenseResult> GetDetailedExpenseById(Guid id, CancellationToken cancellationToken = default)
    {
        var ctx = LogContext.For<ExpensesService>();
        var tenantId = currentUserService.TenantId!.Value;
        var employeeId = currentUserService.EmployeeId ?? Guid.Empty;

        ApplicationLogs.OperationStarted(logger, ctx, tenantId, employeeId);

        var result = DetailedExpenseResult.Create();

        try
        {
            var expenses = (await _expensesRepository.GetAllWithFiltersAsync( exp => exp.Id == id && exp.TenantId == tenantId, cancellationToken, exp => exp.Employee!, exp => exp.Tenant!))
                .ToList();

            if (!expenses.Any())
            {
                result.SetError("Expense not found.", ErrorType.NotFound);
                
                ApplicationLogs.OperationRejected(logger, ctx, tenantId, "Expense not found", StatusCodes.Status404NotFound, id.ToString());
                
                return result;
            }

            result.Data = DetailedExpenseDto.FromModel(expenses[0]);
            
            ApplicationLogs.OperationCompleted(logger, ctx, tenantId, employeeId, detail: $"expenseId={id}");
        }
        catch (Exception ex)
        {
            ApplicationLogs.OperationFailedUnexpectedly(logger, ex, ctx, tenantId, nameof(GetDetailedExpenseById));
            
            result.SetError(OperationMessages.UnexpectedError, ErrorType.Exception);
        }

        return result;
    }
}
