namespace ER.Domain.Shared;

public record GenerateTokenRequest(Guid EmployeeId, Guid TenantId, string Email, EmployeeRole Role);