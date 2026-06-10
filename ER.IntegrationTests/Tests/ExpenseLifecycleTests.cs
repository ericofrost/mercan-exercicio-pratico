using ER.IntegrationTests.Builders;
using ER.IntegrationTests.Fixtures;
using ER.IntegrationTests.Helpers;

namespace ER.IntegrationTests.Tests;

public class ExpenseLifecycleTests(IntegrationTestWebApplicationFactory factory) : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly AuthApiClient _auth = new(factory);

    [Fact]
    public async Task ExpenseLifecycle_SubmitApproveAndRetrieveDetails_Succeeds()
    {
        const decimal amount = 125.50m;

        var employeeClient = await _auth.CreateAuthenticatedClientAsync(SampleEmployeeData.AcmeEmployee1);
        var employeeExpenses = new ExpenseApiClient(employeeClient);

        var submitRequest = ExpenseRequestBuilder.ForAcmeEmployee1().WithAmount(amount).Build();
        var submitResponse = await employeeExpenses.SubmitExpenseAsync(submitRequest);
        submitResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var managerClient = await _auth.CreateAuthenticatedClientAsync(SampleEmployeeData.AcmeManager);
        var managerExpenses = new ExpenseApiClient(managerClient);

        var pending = await managerExpenses.GetPendingExpensesAsync();
        var expense = pending.Expenses.Single(e => e.Amount == amount);

        var approveResponse = await managerExpenses.ApproveExpenseAsync(expense.Id!.Value);
        approveResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var details = await managerExpenses.GetExpenseDetailsAsync(expense.Id!.Value);
        details.Status.Should().Be(ExpenseStatus.Approved);
        details.DecidedByEmployeeId.Should().Be(SampleEmployeeData.AcmeManager.Id);
        details.Tenant.Should().NotBeNull();
        details.Employee.Should().NotBeNull();
        details.Amount.Should().Be(amount);
    }
}
