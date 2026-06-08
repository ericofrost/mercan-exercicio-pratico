namespace ER.Application.Common.Pagination;

public record PagedResult<T>(int TotalCount, int Page, int PageSize, IReadOnlyList<T> Items)
{
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}