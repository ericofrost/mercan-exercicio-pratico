using ER.Domain.Base;

namespace ER.Domain.Models;

/// <summary>
/// Represents a tenant (company) in the multi-tenant system.
/// </summary>
public class Tenant : Model
{
    /// <summary>
    /// Maximum total approved expense amount allowed per calendar month for this tenant.
    /// </summary>
    public decimal MonthlyExpenseLimit { get; set; }
}
