using ER.Application.Authentication;
using FluentValidation;

namespace ER.Application.Validators;

public class LoginValidator : ServiceValidator<LoginRequest,LoginResponse>
{
    public LoginValidator()
    {
        RuleFor(r => r.Email).NotNull().NotEmpty();
        RuleFor(r => r.TenantId).NotNull().NotEmpty();
        RuleFor(r => r.Password).NotNull().NotEmpty();
    }
}