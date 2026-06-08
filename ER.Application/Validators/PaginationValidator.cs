namespace ER.Application.Validators;

/// <summary>
/// Validates paging bounds for <see cref="PaginationQuery"/> requests.
/// </summary>
public class PaginationValidator : AbstractValidator<PaginationQuery>
{
    public PaginationValidator()
    {
        RuleFor(q => q.Page).GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal to 1");
        RuleFor(q => q.PageSize).ExclusiveBetween(1,100) .WithMessage("PageSize must be between 1 and 100.");
    }
}