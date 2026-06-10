using ER.Application.Services.Expenses;

namespace ER.Application.UnitTests.Builders;

public sealed class ChangeExpenseStatusDtoBuilder
{
    private Guid _expenseId = ExpenseBuilder.DefaultExpenseId;
    private ExpenseStatus _status = ExpenseStatus.Approved;
    private string? _rejectReason;

    public ChangeExpenseStatusDtoBuilder WithExpenseId(Guid expenseId)
    {
        _expenseId = expenseId;
        return this;
    }

    public ChangeExpenseStatusDtoBuilder AsApprove()
    {
        _status = ExpenseStatus.Approved;
        _rejectReason = null;
        return this;
    }

    public ChangeExpenseStatusDtoBuilder AsReject(string reason)
    {
        _status = ExpenseStatus.Rejected;
        _rejectReason = reason;
        return this;
    }

    public ChangeExpenseStatusDto Build() => new(_expenseId, _status, _rejectReason);
}
