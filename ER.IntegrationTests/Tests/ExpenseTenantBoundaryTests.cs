using ER.IntegrationTests.Builders;
using ER.IntegrationTests.Fixtures;
using ER.IntegrationTests.Helpers;

namespace ER.IntegrationTests.Tests;

public class ExpenseTenantBoundaryTests(IntegrationTestWebApplicationFactory factory) : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly AuthApiClient _auth = new(factory);

    [Fact]
    public async Task GetExpenseDetails_WhenExpenseBelongsToAnotherTenant_ReturnsNotFound()
    {
        const decimal amount = 210.75m;
        var expenseId = await CreateAcmePendingExpenseAsync(amount);

        var globexClient = await _auth.CreateAuthenticatedClientAsync(SampleEmployeeData.GlobexManager);
        var response = await new ExpenseApiClient(globexClient).GetExpenseDetailsRawAsync(expenseId);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ApproveExpense_WhenExpenseBelongsToAnotherTenant_ReturnsBadRequestAndStaysPending()
    {
        const decimal amount = 310.25m;
        var expenseId = await CreateAcmePendingExpenseAsync(amount);

        var globexClient = await _auth.CreateAuthenticatedClientAsync(SampleEmployeeData.GlobexManager);
        var approveResponse = await new ExpenseApiClient(globexClient).ApproveExpenseAsync(expenseId);

        approveResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var acmeManagerClient = await _auth.CreateAuthenticatedClientAsync(SampleEmployeeData.AcmeManager);
        var pending = await new ExpenseApiClient(acmeManagerClient).GetPendingExpensesAsync();
        pending.Expenses.Should().Contain(e => e.Id == expenseId && e.Amount == amount);
    }

    private async Task<Guid> CreateAcmePendingExpenseAsync(decimal amount)
    {
        var employeeClient = await _auth.CreateAuthenticatedClientAsync(SampleEmployeeData.AcmeEmployee1);
        var employeeExpenses = new ExpenseApiClient(employeeClient);

        var submitRequest = ExpenseRequestBuilder.ForAcmeEmployee1().WithAmount(amount).Build();
        var submitResponse = await employeeExpenses.SubmitExpenseAsync(submitRequest);
        submitResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var managerClient = await _auth.CreateAuthenticatedClientAsync(SampleEmployeeData.AcmeManager);
        var pending = await new ExpenseApiClient(managerClient).GetPendingExpensesAsync();

        return pending.Expenses.Single(e => e.Amount == amount).Id!.Value;
    }
}
