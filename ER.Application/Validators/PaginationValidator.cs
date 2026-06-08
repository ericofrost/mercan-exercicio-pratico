using ER.Application.Common.Pagination;
using FluentValidation;

namespace ER.Application.Validators;

public class PaginationValidator : AbstractValidator<PaginationQuery>
{
    public PaginationValidator()
    {
        RuleFor(q => q.Page).GreaterThanOrEqualTo(1).WithMessage("Page must be greater than or equal to 1");
        RuleFor(q => q.PageSize).ExclusiveBetween(1,100) .WithMessage("PageSize must be between 1 and 100.");
    }
}