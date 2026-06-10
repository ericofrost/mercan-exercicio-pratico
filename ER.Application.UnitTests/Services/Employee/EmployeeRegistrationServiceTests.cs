namespace ER.Application.UnitTests.Services.Employee;

public class EmployeeRegistrationServiceTests
{
    private readonly EmployeeRegistrationServiceFixture _fixture = new();

    [Fact]
    public async Task RegisterAsync_ValidationFails_ReturnsFailureWithoutStartingTransaction()
    {
        var request = new RegisterEmployeeRequestBuilder().Build();
        RegisterEmployeeValidatorMockConfigurator.ValidationFails(_fixture.Validator);
        var sut = _fixture.CreateSut();

        var result = await sut.RegisterAsync(request);

        result.Success.Should().BeFalse();
        _fixture.UnitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Never);
    }

    [Fact]
    public async Task RegisterAsync_IdentityCreateFails_RollsBackAndReturnsFailure()
    {
        var request = new RegisterEmployeeRequestBuilder().Build();
        RegisterEmployeeValidatorMockConfigurator.ValidationSucceeds(_fixture.Validator);
        UserManagerMockConfigurator.SetupCreateAsyncFailure(_fixture.UserManager, "DuplicateUserName");
        var sut = _fixture.CreateSut();

        var result = await sut.RegisterAsync(request);

        result.Success.Should().BeFalse();
        result.Error.Should().ContainSingle(e => e.ErrorType == ErrorType.Service && e.ErrorMessage == "User creation failed.");
        _fixture.UnitOfWork.Verify(u => u.RollbackTransactionAsync(), Times.Once);
        _fixture.UnitOfWork.Verify(u => u.CommitTransactionAsync(), Times.Never);
    }

    [Fact]
    public async Task RegisterAsync_ValidRequest_CommitsAndReturnsIds()
    {
        var request = new RegisterEmployeeRequestBuilder().Build();
        RegisterEmployeeValidatorMockConfigurator.ValidationSucceeds(_fixture.Validator);
        UserManagerMockConfigurator.SetupCreateAsyncSuccess(_fixture.UserManager);
        var sut = _fixture.CreateSut();

        var result = await sut.RegisterAsync(request);

        result.Success.Should().BeTrue();
        result.Data!.EmployeeId.Should().NotBeEmpty();
        result.Data.UserId.Should().NotBeEmpty();
        _fixture.UnitOfWork.Verify(u => u.CommitTransactionAsync(), Times.Once);
        _fixture.EmployeeRepository.Verify(r => r.AddAsync(It.IsAny<Domain.Models.Employee>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_UnexpectedException_RollsBackAndReturnsExceptionError()
    {
        var request = new RegisterEmployeeRequestBuilder().Build();
        RegisterEmployeeValidatorMockConfigurator.ValidationSucceeds(_fixture.Validator);
        UnitOfWorkMockConfigurator.SetupSaveChangesThrows(_fixture.UnitOfWork, new InvalidOperationException("Save failed."));
        var sut = _fixture.CreateSut();

        var result = await sut.RegisterAsync(request);

        result.Success.Should().BeFalse();
        result.Error.Should().ContainSingle(e => e.ErrorType == ErrorType.Exception && e.ErrorMessage == OperationMessages.UnexpectedError);
        _fixture.UnitOfWork.Verify(u => u.RollbackTransactionAsync(), Times.Once);
    }
}
