namespace ER.Domain.Specifications;

/// <summary>
/// Immutable input model used to create or describe an <see cref="Models.Employee"/>.
/// </summary>
/// <param name="TenantId">The owning tenant identifier.</param>
/// <param name="Name">The employee display name.</param>
/// <param name="Email">The employee email address.</param>
/// <param name="Role">The employee role within the tenant.</param>
/// <param name="IsActive">Optional active flag. Defaults to <c>false</c> when omitted in the record default.</param>
/// <param name="Tenant">Optional navigation to the owning tenant.</param>
/// <param name="Expenses">Optional collection of related expenses.</param>
public sealed record EmployeeSpecification(Guid TenantId, string Name, string Email, EmployeeRole Role, bool? IsActive = false, Tenant? Tenant = null, IEnumerable<Expense>? Expenses = null);
