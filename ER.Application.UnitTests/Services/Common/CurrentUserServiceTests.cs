namespace ER.Application.UnitTests.Services.Common;

public class CurrentUserServiceTests
{
    private readonly CurrentUserServiceFixture _fixture = new();

    [Fact]
    public void IsAuthenticated_NoHttpContext_ReturnsFalse()
    {
        _fixture.SetNoHttpContext();
        var sut = _fixture.CreateSut();

        sut.IsAuthenticated.Should().BeFalse();
    }

    [Fact]
    public void EmployeeId_WithSubClaim_ReturnsParsedGuid()
    {
        var employeeId = Guid.Parse("22222222-2222-2222-2222-222222222201");
        _fixture.SetPrincipal(new ClaimsPrincipalBuilder().WithEmployeeId(employeeId).Build());
        var sut = _fixture.CreateSut();

        sut.EmployeeId.Should().Be(employeeId);
    }

    [Fact]
    public void EmployeeId_WithNameIdentifierClaim_ReturnsParsedGuid()
    {
        var employeeId = Guid.Parse("22222222-2222-2222-2222-222222222202");
        _fixture.SetPrincipal(new ClaimsPrincipalBuilder().WithNameIdentifier(employeeId).Build());
        var sut = _fixture.CreateSut();

        sut.EmployeeId.Should().Be(employeeId);
    }

    [Fact]
    public void TenantId_WithTenantClaim_ReturnsParsedGuid()
    {
        var tenantId = Guid.Parse("11111111-1111-1111-1111-111111111101");
        _fixture.SetPrincipal(new ClaimsPrincipalBuilder().WithTenantId(tenantId).Build());
        var sut = _fixture.CreateSut();

        sut.TenantId.Should().Be(tenantId);
    }

    [Fact]
    public void Role_WithRoleClaim_ReturnsParsedEnum()
    {
        _fixture.SetPrincipal(new ClaimsPrincipalBuilder().WithRole(EmployeeRole.Manager).Build());
        var sut = _fixture.CreateSut();

        sut.Role.Should().Be(EmployeeRole.Manager);
    }

    [Fact]
    public void Properties_WhenUnauthenticated_ReturnNulls()
    {
        _fixture.SetPrincipal(new ClaimsPrincipalBuilder().Unauthenticated().Build());
        var sut = _fixture.CreateSut();

        sut.IsAuthenticated.Should().BeFalse();
        sut.EmployeeId.Should().BeNull();
        sut.TenantId.Should().BeNull();
        sut.Role.Should().BeNull();
    }
}
