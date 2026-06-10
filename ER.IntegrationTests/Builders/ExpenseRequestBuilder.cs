namespace ER.IntegrationTests.Builders;

public sealed class ExpenseRequestBuilder
{
    private Guid _tenantId = SampleTenantData.Acme.Id;
    private Guid _employeeId = SampleEmployeeData.AcmeEmployee1.Id;
    private string _employeeName = "Acme Employee One";
    private decimal _amount = 50m;
    private DateOnly _expenseDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
    private DateTime _submittedAt = DateTime.UtcNow;
    private string _description = "Integration test expense submission with sufficient length for API validation.";

    public static ExpenseRequestBuilder ForAcmeEmployee1() => ForEmployee(SampleEmployeeData.AcmeEmployee1);

    public static ExpenseRequestBuilder ForEmployee(Employee employee)
        => new ExpenseRequestBuilder()
            .WithEmployee(employee);

    public ExpenseRequestBuilder WithEmployee(Employee employee)
    {
        _tenantId = employee.TenantId;
        _employeeId = employee.Id;
        _employeeName = employee.Name;
        return this;
    }

    public ExpenseRequestBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public ExpenseRequestBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public SubmitExpenseRequest Build()
        => new(
            null,
            _tenantId,
            _employeeId,
            _amount,
            _expenseDate,
            _submittedAt,
            _employeeName,
            Currency.Eur,
            ExpenseCategory.Meal,
            _description,
            ExpenseStatus.Pending);
}
