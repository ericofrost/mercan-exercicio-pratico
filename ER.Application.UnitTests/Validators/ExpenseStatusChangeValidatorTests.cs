namespace ER.Application.UnitTests.Validators;

public class ExpenseStatusChangeValidatorTests
{
    private readonly ExpenseStatusChangeValidatorFixture _fixture = new();

    [Fact]
    public async Task SetValidationResulWithComparerAsync_WhenUserIsEmployee_ReturnsInvalidWithManagerMessage()
    {
        CurrentUserServiceMockConfigurator.AsEmployee(
            _fixture.CurrentUserService,
            TenantBuilder.DefaultTenantId,
            ExpenseBuilder.DefaultEmployeeId);

        var expense = new ExpenseBuilder().Build();
        var request = new ChangeExpenseStatusDtoBuilder().AsApprove().Build();
        var result = Result<bool>.Create();
        var sut = _fixture.CreateSut();

        var isValid = await sut.SetValidationResulWithComparerAsync(result, request, expense);

        isValid.Should().BeFalse();
        result.Validation!.Errors.Should().ContainSingle(e =>
            e.ErrorMessage == "User needs to be a manager.");
    }

    [Fact]
    public async Task SetValidationResulWithComparerAsync_WhenManagerFromDifferentTenant_ReturnsInvalid()
    {
        CurrentUserServiceMockConfigurator.AsManager(
            _fixture.CurrentUserService,
            TenantBuilder.DefaultTenantId,
            ExpenseBuilder.DefaultManagerId);

        var expense = new ExpenseBuilder().WithTenantId(TenantBuilder.OtherTenantId).Build();
        var request = new ChangeExpenseStatusDtoBuilder().AsReject(new string('x', 10)).Build();
        var result = Result<bool>.Create();
        var sut = _fixture.CreateSut();

        var isValid = await sut.SetValidationResulWithComparerAsync(result, request, expense);

        isValid.Should().BeFalse();
        result.Validation!.Errors.Should().Contain(e =>
            e.ErrorMessage == "User needs to be a manager.");
    }

    [Fact]
    public async Task SetValidationResulWithComparerAsync_WhenManagerApprovesOwnExpense_ReturnsInvalidWithSelfApproveMessage()
    {
        CurrentUserServiceMockConfigurator.AsManager(
            _fixture.CurrentUserService,
            TenantBuilder.DefaultTenantId,
            ExpenseBuilder.DefaultManagerId);

        var expense = new ExpenseBuilder().WithEmployeeId(ExpenseBuilder.DefaultManagerId).Build();
        var request = new ChangeExpenseStatusDtoBuilder().AsApprove().Build();
        var result = Result<bool>.Create();
        var sut = _fixture.CreateSut();

        var isValid = await sut.SetValidationResulWithComparerAsync(result, request, expense);

        isValid.Should().BeFalse();
        result.Validation!.Errors.Should().ContainSingle(e =>
            e.ErrorMessage == "You can't approve your own expense");
    }

    [Fact]
    public async Task SetValidationResulWithComparerAsync_WhenManagerApprovesOtherEmployeeExpense_ReturnsValid()
    {
        CurrentUserServiceMockConfigurator.AsManager(
            _fixture.CurrentUserService,
            TenantBuilder.DefaultTenantId,
            ExpenseBuilder.DefaultManagerId);

        var expense = new ExpenseBuilder().WithEmployeeId(ExpenseBuilder.DefaultEmployeeId).Build();
        var request = new ChangeExpenseStatusDtoBuilder().AsApprove().Build();
        var result = Result<bool>.Create();
        var sut = _fixture.CreateSut();

        var isValid = await sut.SetValidationResulWithComparerAsync(result, request, expense);

        isValid.Should().BeTrue();
        result.Validation!.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task SetValidationResulWithComparerAsync_WhenExpenseAlreadyApproved_ReturnsInvalidWithStatusMessage()
    {
        CurrentUserServiceMockConfigurator.AsManager(
            _fixture.CurrentUserService,
            TenantBuilder.DefaultTenantId,
            ExpenseBuilder.DefaultManagerId);

        var expense = new ExpenseBuilder().WithStatus(ExpenseStatus.Approved).Build();
        var request = new ChangeExpenseStatusDtoBuilder().AsApprove().Build();
        var result = Result<bool>.Create();
        var sut = _fixture.CreateSut();

        var isValid = await sut.SetValidationResulWithComparerAsync(result, request, expense);

        isValid.Should().BeFalse();
        result.Validation!.Errors.Should().ContainSingle(e =>
            e.ErrorMessage == $"It's not possible to change the expense status anymore. Current Status: {ExpenseStatus.Approved}");
    }

    [Fact]
    public async Task SetValidationResulWithComparerAsync_WhenExpenseAlreadyRejected_ReturnsInvalidWithStatusMessage()
    {
        CurrentUserServiceMockConfigurator.AsManager(
            _fixture.CurrentUserService,
            TenantBuilder.DefaultTenantId,
            ExpenseBuilder.DefaultManagerId);

        var expense = new ExpenseBuilder().WithStatus(ExpenseStatus.Rejected).Build();
        var request = new ChangeExpenseStatusDtoBuilder().AsReject(new string('x', 10)).Build();
        var result = Result<bool>.Create();
        var sut = _fixture.CreateSut();

        var isValid = await sut.SetValidationResulWithComparerAsync(result, request, expense);

        isValid.Should().BeFalse();
        result.Validation!.Errors.Should().ContainSingle(e =>
            e.ErrorMessage == $"It's not possible to change the expense status anymore. Current Status: {ExpenseStatus.Rejected}");
    }

    [Fact]
    public async Task SetValidationResulWithComparerAsync_WhenRejectReasonTooShort_ReturnsInvalid()
    {
        CurrentUserServiceMockConfigurator.AsManager(
            _fixture.CurrentUserService,
            TenantBuilder.DefaultTenantId,
            ExpenseBuilder.DefaultManagerId);

        var expense = new ExpenseBuilder().Build();
        var request = new ChangeExpenseStatusDtoBuilder().AsReject(new string('x', 9)).Build();
        var result = Result<bool>.Create();
        var sut = _fixture.CreateSut();

        var isValid = await sut.SetValidationResulWithComparerAsync(result, request, expense);

        isValid.Should().BeFalse();
        result.Validation!.Errors.Should().Contain(e =>
            e.PropertyName.Contains("RejectReason", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task SetValidationResulWithComparerAsync_WhenRejectReasonTooLong_ReturnsInvalid()
    {
        CurrentUserServiceMockConfigurator.AsManager(
            _fixture.CurrentUserService,
            TenantBuilder.DefaultTenantId,
            ExpenseBuilder.DefaultManagerId);

        var expense = new ExpenseBuilder().Build();
        var request = new ChangeExpenseStatusDtoBuilder().AsReject(new string('x', 501)).Build();
        var result = Result<bool>.Create();
        var sut = _fixture.CreateSut();

        var isValid = await sut.SetValidationResulWithComparerAsync(result, request, expense);

        isValid.Should().BeFalse();
        result.Validation!.Errors.Should().Contain(e =>
            e.PropertyName.Contains("RejectReason", StringComparison.OrdinalIgnoreCase));
    }
}
