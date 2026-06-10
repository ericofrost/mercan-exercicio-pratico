using ApiExpense = ER.WebApi.Controllers.Expenses.Expense;

namespace ER.WebApi.Mappings;

/// <summary>
/// AutoMapper profile for expense API contracts and application DTOs.
/// </summary>
public class ExpensesMappingProfile : Profile
{
    public ExpensesMappingProfile()
    {
        CreateMap<ExpenseDto, ApiExpense>();
        CreateMap<SubmitExpenseRequest, SubmitExpenseRequestDto>();
        CreateMap<List<ExpenseDto>, GetPendingExpensesResponse>()
            .ConstructUsing((src, ctx) => new GetPendingExpensesResponse(ctx.Mapper.Map<IEnumerable<ApiExpense>>(src)));
    }
}
