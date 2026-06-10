namespace ER.IntegrationTests.Helpers;

public class ExpenseApiClient(HttpClient client)
{
    public Task<HttpResponseMessage> SubmitExpenseAsync(SubmitExpenseRequest request, CancellationToken cancellationToken = default)
        => client.PostAsJsonAsync("/api/expenses", request, cancellationToken);

    public async Task<GetPendingExpensesResponse> GetPendingExpensesAsync(CancellationToken cancellationToken = default)
    {
        var response = await client.GetAsync("/api/expenses/pending", cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<GetPendingExpensesResponse>(IntegrationTestJson.Options, cancellationToken))!;
    }

    public Task<HttpResponseMessage> ApproveExpenseAsync(Guid expenseId, CancellationToken cancellationToken = default)
        => client.PostAsync($"/api/expenses/{expenseId}/approve", null, cancellationToken);

    public Task<HttpResponseMessage> GetExpenseDetailsRawAsync(Guid expenseId, CancellationToken cancellationToken = default)
        => client.GetAsync($"/api/expenses/{expenseId}", cancellationToken);

    public async Task<DetailedExpense> GetExpenseDetailsAsync(Guid expenseId, CancellationToken cancellationToken = default)
    {
        var response = await GetExpenseDetailsRawAsync(expenseId, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<DetailedExpense>(IntegrationTestJson.Options, cancellationToken))!;
    }
}
