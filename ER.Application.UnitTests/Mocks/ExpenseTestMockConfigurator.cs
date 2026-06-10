using ER.Application.Services.Expenses;

namespace ER.Application.UnitTests.Mocks;

public static class CurrentUserServiceMockConfigurator
{
    public static void AsManager(Mock<ICurrentUserService> currentUserService, Guid tenantId, Guid employeeId)
    {
        currentUserService.SetupGet(u => u.TenantId).Returns(tenantId);
        currentUserService.SetupGet(u => u.EmployeeId).Returns(employeeId);
        currentUserService.SetupGet(u => u.Role).Returns(EmployeeRole.Manager);
        currentUserService.SetupGet(u => u.IsAuthenticated).Returns(true);
    }

    public static void AsEmployee(Mock<ICurrentUserService> currentUserService, Guid tenantId, Guid employeeId)
    {
        currentUserService.SetupGet(u => u.TenantId).Returns(tenantId);
        currentUserService.SetupGet(u => u.EmployeeId).Returns(employeeId);
        currentUserService.SetupGet(u => u.Role).Returns(EmployeeRole.Employee);
        currentUserService.SetupGet(u => u.IsAuthenticated).Returns(true);
    }
}

public static class ExpenseStatusChangeValidatorMockConfigurator
{
    public static void ValidationSucceeds(Mock<IServiceValidatorModelComparer<ChangeExpenseStatusDto, Expense, bool>> validator)
    {
        validator
            .Setup(v => v.SetValidationResulWithComparerAsync(
                It.IsAny<Result<bool>>(),
                It.IsAny<ChangeExpenseStatusDto>(),
                It.IsAny<Expense>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
    }

    public static void ValidationFails(
        Mock<IServiceValidatorModelComparer<ChangeExpenseStatusDto, Expense, bool>> validator,
        string propertyName = nameof(ChangeExpenseStatusDto.ExpenseId),
        string message = "Validation failed.")
    {
        validator
            .Setup(v => v.SetValidationResulWithComparerAsync(
                It.IsAny<Result<bool>>(),
                It.IsAny<ChangeExpenseStatusDto>(),
                It.IsAny<Expense>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Result<bool> result, ChangeExpenseStatusDto _, Expense _, CancellationToken _) =>
            {
                result.Validation = new ValidationResult([new ValidationFailure(propertyName, message)]);
                result.Success = false;
                return false;
            });
    }
}
