namespace ER.Application.UnitTests.Builders;

public sealed class ExpenseBuilder
{
    public static readonly Guid DefaultExpenseId = Guid.Parse("33333333-3333-3333-3333-333333333301");
    public static readonly Guid DefaultEmployeeId = Guid.Parse("22222222-2222-2222-2222-222222222202");
    public static readonly Guid DefaultManagerId = Guid.Parse("22222222-2222-2222-2222-222222222201");

    private Guid _id = DefaultExpenseId;
    private Guid _tenantId = TenantBuilder.DefaultTenantId;
    private Guid _employeeId = DefaultEmployeeId;
    private decimal _amount = 100m;
    private DateOnly _expenseDate = DateOnly.FromDateTime(DateTime.UtcNow);
    private DateTime _submittedAt = DateTime.UtcNow;
    private ExpenseStatus _status = ExpenseStatus.Pending;
    private Tenant? _tenant;

    public ExpenseBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public ExpenseBuilder WithTenantId(Guid tenantId)
    {
        _tenantId = tenantId;
        return this;
    }

    public ExpenseBuilder WithEmployeeId(Guid employeeId)
    {
        _employeeId = employeeId;
        return this;
    }

    public ExpenseBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public ExpenseBuilder WithExpenseDate(DateOnly expenseDate)
    {
        _expenseDate = expenseDate;
        return this;
    }

    public ExpenseBuilder WithStatus(ExpenseStatus status)
    {
        _status = status;
        return this;
    }

    public ExpenseBuilder WithTenant(Tenant tenant)
    {
        _tenant = tenant;
        _tenantId = tenant.Id;
        return this;
    }

    public Expense Build()
    {
        var tenant = _tenant ?? new TenantBuilder().WithId(_tenantId).Build();
        var employee = new EmployeeBuilder().WithId(_employeeId).WithTenantId(_tenantId).Build();

        return new Expense(
            _id,
            _tenantId,
            _employeeId,
            _amount,
            _expenseDate,
            _submittedAt,
            tenant,
            employee,
            status: _status);
    }
}
