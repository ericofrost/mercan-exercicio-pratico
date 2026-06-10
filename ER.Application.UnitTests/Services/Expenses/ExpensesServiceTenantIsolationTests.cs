namespace ER.Application.UnitTests.Services.Expenses;

public class ExpensesServiceTenantIsolationTests
{
    private readonly ExpensesServiceFixture _fixture = new();
    private readonly SubmitExpenseValidatorFixture _submitFixture = new();

    [Fact]
    public async Task GetDetailedExpenseById_WhenExpenseBelongsToAnotherTenant_ReturnsNotFound()
    {
        var expenseInOtherTenant = new ExpenseBuilder().WithTenantId(TenantBuilder.OtherTenantId).Build();

        CurrentUserServiceMockConfigurator.AsManager(
            _fixture.CurrentUserService,
            TenantBuilder.DefaultTenantId,
            ExpenseBuilder.DefaultManagerId);
        UnitOfWorkMockConfigurator.SetupGetAllWithFiltersAsync(
            _fixture.ExpenseRepository,
            new[] { expenseInOtherTenant });

        var sut = _fixture.CreateSut();

        var result = await sut.GetDetailedExpenseById(expenseInOtherTenant.Id);

        result.Success.Should().BeFalse();
        result.Error.Should().ContainSingle(e => e.ErrorType == ErrorType.NotFound);
    }

    [Fact]
    public async Task SubmitExpenseValidator_WhenRequestTenantDiffersFromAuthenticatedUser_ReturnsInvalid()
    {
        CurrentUserServiceMockConfigurator.AsEmployee(
            _submitFixture.CurrentUserService,
            TenantBuilder.DefaultTenantId,
            ExpenseBuilder.DefaultEmployeeId);
        UnitOfWorkMockConfigurator.SetupExistsWithFilterAsync(_submitFixture.Repositories.TenantRepository, true);
        UnitOfWorkMockConfigurator.SetupExistsWithFilterAsync(_submitFixture.Repositories.EmployeeRepository, true);
        UnitOfWorkMockConfigurator.SetupExistsWithFilterAsync(_submitFixture.Repositories.ExpenseRepository, false);

        var request = new SubmitExpenseRequestDtoBuilder().WithTenantId(TenantBuilder.OtherTenantId).Build();
        var result = Result<bool>.Create();
        var sut = _submitFixture.CreateSut();

        var isValid = await sut.SetValidationResultAsync(result, request);

        isValid.Should().BeFalse();
        result.Validation!.Errors.Should().Contain(e =>
            e.PropertyName == nameof(SubmitExpenseRequestDto.TenantId) &&
            e.ErrorMessage == "Expense must belong to the authenticated tenant.");
    }
}
