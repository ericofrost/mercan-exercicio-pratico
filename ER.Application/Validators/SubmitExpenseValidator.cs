namespace ER.Application.Validators;

public class SubmitExpenseValidator : ServiceValidator<SubmitExpenseRequestDto, bool>
{
    private readonly IGenericRepository<Expense> _repository;

    public SubmitExpenseValidator(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : base(unitOfWork)
    {
        _repository = unitOfWork.Repository<Expense>();
        
        RuleFor(x => x).NotNull();
        RuleFor(x => x).MustAsync(ExpenseNotExist).WithMessage("Expense already exists");
        RuleFor(x => x.TenantId).MustAsync(TenantExistsAsync).WithMessage("Tenant must exist");
        RuleFor(x => x.TenantId)
            .Must(tenantId => tenantId == currentUserService.TenantId)
            .WithMessage("Expense must belong to the authenticated tenant.");
        RuleFor(x => x).MustAsync(EmployeeExistsInTenantAsync).WithMessage("Employee must exist");
        RuleFor(x => x.EmployeeId)
            .Must(employeeId => employeeId == currentUserService.EmployeeId)
            .WithMessage("Expense must belong to the authenticated employee.");
        RuleFor(x => x).Must(x => x.ExpenseDate > DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-90))).WithMessage("Expense Date cannot be older than 90 days.");
        RuleFor(x => x).Must(x => x.ExpenseDate < DateOnly.FromDateTime(x.SubmittedAt)).WithMessage("Expense Date cannot be before Expense Date");
        RuleFor(x => x.Status).Equal(ExpenseStatus.Pending).WithMessage("Expense Status must be pending.");
    }
    
    private async Task<bool> ExpenseNotExist(SubmitExpenseRequestDto expenseRequestDto, CancellationToken cancellationToken = default)
    {
        return !(await _repository.ExistsWithFilterAsync(
            e => e.TenantId == expenseRequestDto.TenantId
                 && e.EmployeeId == expenseRequestDto.EmployeeId
                 && e.Amount == expenseRequestDto.Amount
                 && e.ExpenseDate == expenseRequestDto.ExpenseDate
                 && e.Currency == expenseRequestDto.Currency
                 && e.Category == expenseRequestDto.Category,
            cancellationToken));
    }
    
    private async Task<bool> EmployeeExistsInTenantAsync(SubmitExpenseRequestDto request, CancellationToken cancellationToken)
    {
        return await EmployeeRepository.ExistsWithFilterAsync(e => e.TenantId == request.TenantId && e.Id == request.EmployeeId, cancellationToken);
    }
}