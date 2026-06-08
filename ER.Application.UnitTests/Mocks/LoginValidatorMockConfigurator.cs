namespace ER.Application.UnitTests.Mocks;

public static class LoginValidatorMockConfigurator
{
    public static void ValidationSucceeds(Mock<IServiceValidator<LoginRequest, LoginResponse>> validator)
    {
        validator
            .Setup(v => v.SetValidationResultAsync(It.IsAny<Result<LoginResponse>>(), It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    }

    public static void ValidationFails(Mock<IServiceValidator<LoginRequest, LoginResponse>> validator, string propertyName = "Email", string message = "Validation failed.")
    {
        validator
            .Setup(v => v.SetValidationResultAsync(It.IsAny<Result<LoginResponse>>(), It.IsAny<LoginRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Result<LoginResponse> result, LoginRequest _, CancellationToken _) =>
            {
                result.Validation = new ValidationResult([new ValidationFailure(propertyName, message)]);
                result.Success = false;
                return false;
            });
    }
}

public static class RegisterEmployeeValidatorMockConfigurator
{
    public static void ValidationSucceeds(Mock<IServiceValidator<RegisterEmployeeRequest, RegisterEmployeeResult>> validator)
    {
        validator
            .Setup(v => v.SetValidationResultAsync(It.IsAny<Result<RegisterEmployeeResult>>(), It.IsAny<RegisterEmployeeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    }

    public static void ValidationFails(Mock<IServiceValidator<RegisterEmployeeRequest, RegisterEmployeeResult>> validator, string propertyName = "TenantId", string message = "Tenant not found or inactive.")
    {
        validator
            .Setup(v => v.SetValidationResultAsync(It.IsAny<Result<RegisterEmployeeResult>>(), It.IsAny<RegisterEmployeeRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Result<RegisterEmployeeResult> result, RegisterEmployeeRequest _, CancellationToken _) =>
            {
                result.Validation = new ValidationResult([new ValidationFailure(propertyName, message)]);
                result.Success = false;
                return false;
            });
    }
}
