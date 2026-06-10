namespace ER.Application.Interfaces.Validators;

public interface IPaginationValidator
{
    Task<bool> SetValidationResultAsync<T>(PagedResult<T> result, PaginationRequestDto data, CancellationToken cancellationToken = default);
}