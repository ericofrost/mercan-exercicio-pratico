namespace ER.Application.UnitTests.Services.Expenses;

public class ExpensesServiceApprovalTests
{
    private readonly ExpensesServiceFixture _fixture = new();

    [Fact]
    public async Task ApproveExpenseAsync_WhenMonthlyLimitExceeded_ReturnsServiceErrorAndRollsBack()
    {
        var tenant = new TenantBuilder().WithMonthlyExpenseLimit(1000m).Build();
        var expense = new ExpenseBuilder().WithAmount(100m).WithTenant(tenant).Build();
        var existingApproved = new ExpenseBuilder()
            .WithId(Guid.NewGuid())
            .WithAmount(950m)
            .WithStatus(ExpenseStatus.Approved)
            .WithExpenseDate(expense.ExpenseDate)
            .WithEmployeeId(expense.EmployeeId)
            .WithTenantId(expense.TenantId)
            .Build();

        CurrentUserServiceMockConfigurator.AsManager(
            _fixture.CurrentUserService,
            tenant.Id,
            ExpenseBuilder.DefaultManagerId);
        ExpenseStatusChangeValidatorMockConfigurator.ValidationSucceeds(_fixture.ApprovalValidator);
        UnitOfWorkMockConfigurator.SetupGetAllWithFiltersAsync(
            _fixture.ExpenseRepository,
            new[] { expense, existingApproved });

        var sut = _fixture.CreateSut();
        var request = new ChangeExpenseStatusDtoBuilder().WithExpenseId(expense.Id).AsApprove().Build();

        var result = await sut.ApproveExpenseAsync(request);

        result.Success.Should().BeFalse();
        result.Error.Should().ContainSingle(e =>
            e.ErrorType == ErrorType.Service &&
            e.ErrorMessage.Contains("Monthly Expense Limit", StringComparison.Ordinal));
        _fixture.UnitOfWork.Verify(u => u.RollbackTransactionAsync(), Times.Once);
        _fixture.ExpenseRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Expense>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task ApproveExpenseAsync_WhenWithinMonthlyLimit_CommitsApproval()
    {
        var tenant = new TenantBuilder().WithMonthlyExpenseLimit(1000m).Build();
        var expense = new ExpenseBuilder().WithAmount(100m).WithTenant(tenant).Build();
        var existingApproved = new ExpenseBuilder()
            .WithId(Guid.NewGuid())
            .WithAmount(200m)
            .WithStatus(ExpenseStatus.Approved)
            .WithExpenseDate(expense.ExpenseDate)
            .WithEmployeeId(expense.EmployeeId)
            .WithTenantId(expense.TenantId)
            .Build();

        CurrentUserServiceMockConfigurator.AsManager(
            _fixture.CurrentUserService,
            tenant.Id,
            ExpenseBuilder.DefaultManagerId);
        ExpenseStatusChangeValidatorMockConfigurator.ValidationSucceeds(_fixture.ApprovalValidator);
        UnitOfWorkMockConfigurator.SetupGetAllWithFiltersAsync(
            _fixture.ExpenseRepository,
            new[] { expense, existingApproved });
        _fixture.ExpenseRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Expense>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var sut = _fixture.CreateSut();
        var request = new ChangeExpenseStatusDtoBuilder().WithExpenseId(expense.Id).AsApprove().Build();

        var result = await sut.ApproveExpenseAsync(request);

        result.Success.Should().BeTrue();
        _fixture.ExpenseRepository.Verify(
            r => r.UpdateAsync(It.IsAny<Expense>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _fixture.UnitOfWork.Verify(u => u.CommitTransactionAsync(), Times.Once);
    }
}
