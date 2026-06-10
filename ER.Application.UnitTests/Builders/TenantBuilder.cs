namespace ER.Application.UnitTests.Builders;

public sealed class TenantBuilder
{
    public static readonly Guid DefaultTenantId = Guid.Parse("11111111-1111-1111-1111-111111111101");
    public static readonly Guid OtherTenantId = Guid.Parse("11111111-1111-1111-1111-111111111102");

    private Guid _id = DefaultTenantId;
    private string _name = "Test Tenant";
    private decimal _monthlyExpenseLimit = 10_000m;
    private bool _isActive = true;

    public TenantBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }

    public TenantBuilder WithMonthlyExpenseLimit(decimal limit)
    {
        _monthlyExpenseLimit = limit;
        return this;
    }

    public TenantBuilder Inactive()
    {
        _isActive = false;
        return this;
    }

    public Tenant Build() => new(_id, _name, _monthlyExpenseLimit, _isActive);
}
