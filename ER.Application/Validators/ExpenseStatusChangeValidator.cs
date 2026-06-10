using ER.Application.Services.Expenses;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace ER.Application.Validators;

public class ExpenseStatusChangeValidator : ServiceValidatorModelComparer<ChangeExpenseStatusDto, Expense, bool>
{
    private readonly ICurrentUserService _currentUserService;

    public ExpenseStatusChangeValidator(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : base(unitOfWork)
    {
        _currentUserService = currentUserService;
        
        RuleFor(x => x).Cascade(CascadeMode.Stop).NotNull();
        RuleFor(x => x).Cascade(CascadeMode.Stop).Custom(ValidateExistingExpense);
        When(x => x.ExpenseStatus == ExpenseStatus.Rejected, () =>
        {
            RuleFor(x => x.RejectReason).Cascade(CascadeMode.Stop).NotNull();
            RuleFor(x => x.RejectReason!.Length).InclusiveBetween(10,500);
        });
    }
    
    private void ValidateExistingExpense(ChangeExpenseStatusDto expenseStatusDto, ValidationContext<ChangeExpenseStatusDto> context)
    {
        if (_currentUserService.Role != EmployeeRole.Manager) context.AddFailure(new ValidationFailure(nameof(ChangeExpenseStatusDto.ExpenseId), "User needs to be a manager.", expenseStatusDto.ExpenseId));
        
        if (_currentUserService.TenantId != Compare.TenantId) context.AddFailure(new ValidationFailure(nameof(ChangeExpenseStatusDto.ExpenseId), "User needs to be a manager.", expenseStatusDto.ExpenseId));
        
        if (Compare.EmployeeId == _currentUserService.EmployeeId && expenseStatusDto.ExpenseStatus == ExpenseStatus.Approved) context.AddFailure(new ValidationFailure(nameof(ChangeExpenseStatusDto.ExpenseId), "You can't approve your own expense", expenseStatusDto.ExpenseId));
        
        if (Compare.Status != ExpenseStatus.Pending) context.AddFailure(new ValidationFailure(nameof(ChangeExpenseStatusDto.ExpenseStatus), $"It's not possible to change the expense status anymore. Current Status: {Compare.Status}", expenseStatusDto.ExpenseId));
    }
}