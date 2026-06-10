using ER.Application.Services.Expenses;

namespace ER.Application.Interfaces.Services;

/// <summary>
/// Application service contract for expense operations. Reserved for future implementation.
/// </summary>
public interface IExpensesService
{
    Task<ExpenseByTenantResult> GetAllExpensesByManagerTenant(CancellationToken cancellationToken = default);
    
    Task<Result<bool>> SubmitExpenseAsync(SubmitExpenseRequestDto expense, CancellationToken cancellationToken = default);
    
    Task<Result<bool>> ApproveExpenseAsync(ChangeExpenseStatusDto approvalRequest, CancellationToken cancellationToken = default);
    
    Task<Result<bool>> RejectExpenseAsync(ChangeExpenseStatusDto rejectRequest, CancellationToken cancellationToken = default);
}