namespace ER.Application.UnitTests.Mocks;

public static class UnitOfWorkMockConfigurator
{
    public sealed record UnitOfWorkMocks(
        Mock<IUnitOfWork> UnitOfWork,
        Mock<IGenericRepository<Employee>> EmployeeRepository,
        Mock<IGenericRepository<Tenant>> TenantRepository);

    public static UnitOfWorkMocks CreateWithRepositories()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var employeeRepository = new Mock<IGenericRepository<Employee>>();
        var tenantRepository = new Mock<IGenericRepository<Tenant>>();

        unitOfWork.Setup(u => u.Repository<Employee>()).Returns(employeeRepository.Object);
        unitOfWork.Setup(u => u.Repository<Tenant>()).Returns(tenantRepository.Object);

        SetupTransaction(unitOfWork);

        return new UnitOfWorkMocks(unitOfWork, employeeRepository, tenantRepository);
    }

    public static void SetupTransaction(Mock<IUnitOfWork> unitOfWork)
    {
        unitOfWork.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
        unitOfWork.Setup(u => u.RollbackTransactionAsync()).Returns(Task.CompletedTask);
    }

    public static void SetupExistsWithFilterAsync<T>(Mock<IGenericRepository<T>> repository, bool exists) where T : Domain.Base.BaseModel
    {
        repository
            .Setup(r => r.ExistsWithFilterAsync(It.IsAny<Expression<Func<T, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exists);
    }

    public static void SetupSaveChangesThrows(Mock<IUnitOfWork> unitOfWork, Exception exception)
    {
        unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);
    }
}
