using ER.Application.Services.Expenses;

namespace ER.Application.UnitTests.Fixtures;

public sealed class ExpensesServiceFixture
{
    public UnitOfWorkMockConfigurator.UnitOfWorkMocks Repositories { get; } = UnitOfWorkMockConfigurator.CreateWithRepositories();

    public Mock<IUnitOfWork> UnitOfWork => Repositories.UnitOfWork;

    public Mock<IGenericRepository<Expense>> ExpenseRepository => Repositories.ExpenseRepository;

    public Mock<IServiceValidator<SubmitExpenseRequestDto, bool>> SubmitValidator { get; } = new();

    public Mock<IPaginationValidator> PaginationValidator { get; } = new();

    public Mock<IServiceValidatorModelComparer<ChangeExpenseStatusDto, Expense, bool>> ApprovalValidator { get; } = new();

    public Mock<ICurrentUserService> CurrentUserService { get; } = new();

    public ExpensesService CreateSut() => new(
        UnitOfWork.Object,
        NullLogger<ExpensesService>.Instance,
        SubmitValidator.Object,
        PaginationValidator.Object,
        ApprovalValidator.Object,
        CurrentUserService.Object);
}

public sealed class ExpenseStatusChangeValidatorFixture
{
    public UnitOfWorkMockConfigurator.UnitOfWorkMocks Repositories { get; } = UnitOfWorkMockConfigurator.CreateWithRepositories();

    public Mock<ICurrentUserService> CurrentUserService { get; } = new();

    public ExpenseStatusChangeValidator CreateSut() => new(
        Repositories.UnitOfWork.Object,
        CurrentUserService.Object);
}

public sealed class SubmitExpenseValidatorFixture
{
    public UnitOfWorkMockConfigurator.UnitOfWorkMocks Repositories { get; } = UnitOfWorkMockConfigurator.CreateWithRepositories();

    public Mock<ICurrentUserService> CurrentUserService { get; } = new();

    public SubmitExpenseValidator CreateSut() => new(
        Repositories.UnitOfWork.Object,
        CurrentUserService.Object);
}
