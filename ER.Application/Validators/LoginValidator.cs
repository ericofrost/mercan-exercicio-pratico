namespace ER.Application.Validators;

/// <summary>
/// Validates required fields on <see cref="LoginRequest"/> before authentication proceeds.
/// </summary>
public class LoginValidator : ServiceValidator<LoginRequest,LoginResponse>
{
    public LoginValidator()
    {
        RuleFor(r => r.Email).NotNull().NotEmpty();
        RuleFor(r => r.TenantId).NotNull().NotEmpty();
        RuleFor(r => r.Password).NotNull().NotEmpty();
    }
}