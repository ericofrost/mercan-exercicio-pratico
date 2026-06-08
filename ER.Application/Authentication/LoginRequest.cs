namespace ER.Application.Authentication;

public record LoginRequest(Guid TenantId, string Email, string Password);
