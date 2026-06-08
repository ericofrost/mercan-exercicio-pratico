namespace ER.Application.Authentication;

public record LoginResponse(string AccessToken, DateTime ExpiresAt);
