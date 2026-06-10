using ER.Application.Services.Expenses;

namespace ER.Application.UnitTests.Builders;

public sealed class SubmitExpenseRequestDtoBuilder
{
    private Guid _tenantId = TenantBuilder.DefaultTenantId;
    private Guid _employeeId = ExpenseBuilder.DefaultEmployeeId;
    private decimal _amount = 50m;
    private DateOnly _expenseDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
    private DateTime _submittedAt = DateTime.UtcNow;

    public SubmitExpenseRequestDtoBuilder WithTenantId(Guid tenantId)
    {
        _tenantId = tenantId;
        return this;
    }

    public SubmitExpenseRequestDtoBuilder WithEmployeeId(Guid employeeId)
    {
        _employeeId = employeeId;
        return this;
    }

    public SubmitExpenseRequestDto Build()
        => new(
            Guid.Empty,
            _tenantId,
            _employeeId,
            _amount,
            _expenseDate,
            _submittedAt,
            "Test Employee",
            Currency.Eur,
            ExpenseCategory.Meal,
            "Integration test expense",
            ExpenseStatus.Pending);
}
