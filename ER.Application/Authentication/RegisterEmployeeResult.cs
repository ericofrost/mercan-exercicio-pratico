namespace ER.Application.Authentication;

/// <summary>
/// Successful employee registration response containing the created identifiers.
/// </summary>
/// <param name="EmployeeId">The created employee identifier.</param>
/// <param name="UserId">The created identity user identifier.</param>
public record RegisterEmployeeResult(Guid EmployeeId, Guid UserId);
