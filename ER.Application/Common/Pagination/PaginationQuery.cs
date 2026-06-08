namespace ER.Application.Common.Pagination;

/// <summary>
/// Common paging parameters for list queries.
/// </summary>
/// <param name="Page">The one-based page number. Defaults to 1.</param>
/// <param name="PageSize">The number of items per page. Defaults to 20.</param>
public record PaginationQuery(int Page = 1, int PageSize = 20);