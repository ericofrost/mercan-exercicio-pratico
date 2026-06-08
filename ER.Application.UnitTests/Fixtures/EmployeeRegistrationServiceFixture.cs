namespace ER.Application.UnitTests.Fixtures;

public sealed class EmployeeRegistrationServiceFixture
{
    public UnitOfWorkMockConfigurator.UnitOfWorkMocks Repositories { get; } = UnitOfWorkMockConfigurator.CreateWithRepositories();

    public Mock<UserManager<ApplicationUser>> UserManager { get; } = UserManagerMockConfigurator.Create();

    public Mock<IServiceValidator<RegisterEmployeeRequest, RegisterEmployeeResult>> Validator { get; } = new();

    public Mock<IUnitOfWork> UnitOfWork => Repositories.UnitOfWork;

    public Mock<IGenericRepository<Employee>> EmployeeRepository => Repositories.EmployeeRepository;

    public Mock<IGenericRepository<Tenant>> TenantRepository => Repositories.TenantRepository;

    public EmployeeRegistrationService CreateSut() => new(
        UnitOfWork.Object,
        UserManager.Object,
        Validator.Object,
        NullLogger<EmployeeRegistrationService>.Instance);
}
