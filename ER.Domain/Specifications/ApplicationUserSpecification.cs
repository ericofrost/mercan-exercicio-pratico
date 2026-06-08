namespace ER.Domain.Specifications;

/// <summary>
/// Immutable input model used to create or describe a <see cref="Shared.ApplicationUser"/>.
/// </summary>
/// <param name="TenantId">The owning tenant identifier.</param>
/// <param name="EmployeeId">The linked employee identifier, also used as the shared primary key.</param>
/// <param name="Email">The user email address.</param>
/// <param name="EmailConfirmed">Whether the email address has been confirmed. Defaults to <c>true</c>.</param>
public sealed record ApplicationUserSpecification(Guid TenantId, Guid EmployeeId, string Email, bool EmailConfirmed = true);
