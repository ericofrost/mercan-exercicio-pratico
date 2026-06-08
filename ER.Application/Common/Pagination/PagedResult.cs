namespace ER.Application.Common.Pagination;

/// <summary>
/// Represents a paginated collection of items with navigation metadata.
/// </summary>
/// <typeparam name="T">The item type.</typeparam>
/// <param name="TotalCount">Total number of items matching the query across all pages.</param>
/// <param name="Page">The current one-based page number.</param>
/// <param name="PageSize">The maximum number of items per page.</param>
/// <param name="Items">The items returned for the current page.</param>
public record PagedResult<T>(int TotalCount, int Page, int PageSize, IReadOnlyList<T> Items)
{
    /// <summary>
    /// Gets the total number of pages based on <see cref="TotalCount"/> and <see cref="PageSize"/>.
    /// </summary>
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;

    /// <summary>
    /// Gets a value indicating whether a subsequent page exists.
    /// </summary>
    public bool HasNextPage => Page < TotalPages;

    /// <summary>
    /// Gets a value indicating whether a previous page exists.
    /// </summary>
    public bool HasPreviousPage => Page > 1;
}