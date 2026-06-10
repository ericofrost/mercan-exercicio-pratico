namespace ER.Application.Common.Pagination;

/// <summary>
/// Common paging and sorting parameters for repository list queries.
/// Maps directly to <see cref="Interfaces.Repositories.IGenericRepository{T}.GetPaginatedListAsync"/>.
/// </summary>
/// <param name="CurrentPage">The one-based page number. Defaults to 1.</param>
/// <param name="RowsPerPage">The number of items per page. Defaults to 20.</param>
/// <param name="OrderBy">The entity property name to sort by. Defaults to <c>Id</c>.</param>
/// <param name="Order">The sort direction: <c>asc</c> or <c>desc</c>. Defaults to <c>asc</c>.</param>
public record PaginationRequestDto(int CurrentPage = 1, int RowsPerPage = 20, string OrderBy = "Id", string Order = "asc")
{
    /// <summary>
    /// Returns the parameter tuple expected by <c>GetPaginatedListAsync</c>.
    /// </summary>
    public (string orderBy, string order, int rowsPerPage, int currentPage) ToRepositoryParameters()
        => (OrderBy, Order, RowsPerPage, CurrentPage);
}
