using ER.Application.Interfaces.Services;

namespace ER.Application.Services.Expenses;

public class ExpensesService(IUnitOfWork unitOfWork, ILogger<ExpensesService> logger, IServiceValidator<SubmitExpenseRequestDto, bool> submitValidator,
    IServiceValidatorModelComparer<ChangeExpenseStatusDto, Expense, bool> approvalValidator, 
    ICurrentUserService currentUserService) : IExpensesService
{
    private readonly IGenericRepository<Expense> _expensesRepository = unitOfWork.Repository<Expense>();
    private readonly LogContext _ctx = LogContext.For<ExpensesService>();
    
    public async Task<ExpenseByTenantResult> GetAllExpensesByManagerTenant(CancellationToken cancellationToken = default)
    {
        var result = ExpenseByTenantResult.Create();
        
        if (currentUserService.Role != EmployeeRole.Manager)
        {
            result.SetError("User is not a manager.", ErrorType.Service);
            
            return result;
        }

        try
        {
            var expenses = await _expensesRepository.GetAllWithFiltersAsync(exp => exp.TenantId == currentUserService.TenantId && exp.Status == ExpenseStatus.Pending, cancellationToken, exp => exp.Employee);
            
            expenses.ToList().ForEach(x =>
            {
                result.Data!.Add(ExpenseDto.FromModel(x));
            });
        }
        catch (Exception ex)
        {
            ApplicationLogs.OperationFailedUnexpectedly(logger, ex, _ctx, currentUserService.TenantId!.Value, nameof(GetAllExpensesByManagerTenant));
            
            result.SetError(ex.Message, ErrorType.Exception);
        }
        
        return result;
    }

    public async Task<Result<bool>> SubmitExpenseAsync(SubmitExpenseRequestDto expense, CancellationToken cancellationToken = default)
    {
        var result = Result<bool>.Create();

        try
        {
            if (!await submitValidator.SetValidationResultAsync(result, expense, cancellationToken))
            {
                return result;
            }

            await unitOfWork.BeginTransactionAsync();

            await _expensesRepository.AddAsync(expense.ToModel(), cancellationToken);

            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            
            ApplicationLogs.OperationFailedUnexpectedly(logger, ex, _ctx, currentUserService.TenantId!.Value, nameof(SubmitExpenseAsync));
            
            result.SetError(ex.Message, ErrorType.Exception);
        }
        
        return result;
    }

    public async Task<Result<bool>> ApproveExpenseAsync(ChangeExpenseStatusDto approvalRequest, CancellationToken cancellationToken = default)
    {
        var result = Result<bool>.Create();
        
        try
        {
            var expense = (await _expensesRepository.GetAllWithFiltersAsync(exp => exp.Id == approvalRequest.ExpenseId, cancellationToken, exp => exp.Tenant)).FirstOrDefault();

            if (expense == null)
            {
                result.SetError("Expense not found.", ErrorType.NotFound);
                
                return result;
            }

            if (await approvalValidator.SetValidationResulWithComparerAsync(result, approvalRequest, expense, cancellationToken)) return result;
            
            await unitOfWork.BeginTransactionAsync();
            
            var totalExpenses = (await _expensesRepository.GetAllWithFiltersAsync(exp => exp.EmployeeId == expense.EmployeeId && expense.TenantId == currentUserService.TenantId, cancellationToken))
                .Sum(e => e.Amount);

            if (expense.ValidateApprovalAmount(totalExpenses))
            {
                await unitOfWork.RollbackTransactionAsync();
                
                result.SetError($"This approval is not possible. Reason: It exceeds the Monthly Expense Limit for the Tenant: {expense.TenantId} and Employee: {expense.EmployeeId}", ErrorType.Service);
                
                return result;
            }
            
            expense.SetApprovalDetails(ExpenseStatus.Approved, currentUserService.EmployeeId!.Value);
            
            await _expensesRepository.UpdateAsync(expense, cancellationToken);
            
            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            
            ApplicationLogs.OperationFailedUnexpectedly(logger, ex, _ctx, currentUserService.TenantId!.Value, nameof(ApproveExpenseAsync));
            
            result.SetError(ex.Message, ErrorType.Exception);
        }
        
        return result;
    }

    public async Task<Result<bool>> RejectExpenseAsync(ChangeExpenseStatusDto rejectRequest, CancellationToken cancellationToken = default)
    {
        var result = Result<bool>.Create();
        
        try
        {
            var expense = (await _expensesRepository.GetAllWithFiltersAsync(exp => exp.Id == rejectRequest.ExpenseId, cancellationToken)).FirstOrDefault();

            if (expense == null)
            {
                result.SetError("Expense not found.", ErrorType.NotFound);
                
                return result;
            }

            if (await approvalValidator.SetValidationResulWithComparerAsync(result, rejectRequest, expense, cancellationToken)) return result;
            
            await unitOfWork.BeginTransactionAsync();
            
            expense.SetApprovalDetails(ExpenseStatus.Rejected, currentUserService.EmployeeId!.Value, rejectRequest.RejectReason);
            
            await _expensesRepository.UpdateAsync(expense, cancellationToken);
            
            await unitOfWork.CommitTransactionAsync();
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            
            ApplicationLogs.OperationFailedUnexpectedly(logger, ex, _ctx, currentUserService.TenantId!.Value, nameof(RejectExpenseAsync));
            
            result.SetError(ex.Message, ErrorType.Exception);
        }
        
        return result;
    }
}