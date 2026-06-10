namespace ER.Application.Validators;

/// <summary>
/// Validates paging and sorting bounds for <see cref="PaginationRequestDto"/> requests.
/// </summary>
public class PaginationValidator : AbstractValidator<PaginationRequestDto>, IPaginationValidator
{
    private static readonly string[] AllowedOrders = ["asc", "desc"];

    public PaginationValidator()
    {
        RuleFor(q => q.CurrentPage).GreaterThanOrEqualTo(1).WithMessage("CurrentPage must be greater than or equal to 1.");
        RuleFor(q => q.RowsPerPage).InclusiveBetween(1, 100).WithMessage("RowsPerPage must be between 1 and 100.");
        RuleFor(q => q.OrderBy).NotEmpty().WithMessage("OrderBy is required.");
        RuleFor(q => q.Order)
            .Must(order => AllowedOrders.Contains(order, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Order must be 'asc' or 'desc'.");
    }
    
    public virtual async Task<bool> SetValidationResultAsync<T>(PagedResult<T> result, PaginationRequestDto data, CancellationToken cancellationToken = default)    
    {
        var validationResult = await this.ValidateAsync(data, cancellationToken);
        
        result.Validation = validationResult;   
        
        return validationResult.IsValid;
    }
}
