namespace ER.Application.Common.Pagination;

/// <summary>
/// Represents a paginated collection of items with navigation metadata.
/// </summary>
/// <typeparam name="T">The item type.</typeparam>
/// <param name="TotalCount">Total number of items matching the query across all pages.</param>
/// <param name="CurrentPage">The current one-based page number.</param>
/// <param name="RowsPerPage">The maximum number of items per page.</param>
/// <param name="Items">The items returned for the current page.</param>
public record PagedResult<T>(int TotalCount, int CurrentPage, int RowsPerPage, IReadOnlyList<T> Items) : IOperationResult
{
    /// <summary>
    /// Gets a value indicating whether the operation completed without validation or application errors.
    /// </summary>
    public bool Success => ((IOperationResult)this).IsSuccessful;
    
    /// <summary>
    /// Gets the total number of pages based on <see cref="TotalCount"/> and <see cref="RowsPerPage"/>.
    /// </summary>
    public int TotalPages => RowsPerPage > 0 ? (int)Math.Ceiling(TotalCount / (double)RowsPerPage) : 0;

    /// <summary>
    /// Gets a value indicating whether a subsequent page exists.
    /// </summary>
    public bool HasNextPage => CurrentPage < TotalPages;

    /// <summary>
    /// Gets a value indicating whether a previous page exists.
    /// </summary>
    public bool HasPreviousPage => CurrentPage > 1;

    /// <summary>
    /// Creates a <see cref="PagedResult{T}"/> from a repository paginated query result.
    /// </summary>
    /// <param name="totalCount">Total matching records returned by the repository.</param>
    /// <param name="items">The page items returned by the repository.</param>
    /// <param name="requestDto">The pagination parameters used for the query.</param>
    public static PagedResult<T> FromRepository(int totalCount, IEnumerable<T> items, PaginationRequestDto requestDto)
        => new(totalCount, requestDto.CurrentPage, requestDto.RowsPerPage, items.ToList());
    
    /// <summary>
    /// Creates a <see cref="PagedResult{T}"/> from a paginated query request.
    /// </summary>
    /// <param name="requestDto">The pagination parameters used for the query.</param>
    public static PagedResult<T> FromPaginatedRequest(PaginationRequestDto requestDto)
        => new(0, requestDto.CurrentPage, requestDto.RowsPerPage, []);
    
    /// <summary>
    /// Gets the collection of application errors populated when the operation fails.
    /// </summary>
    public List<Error> Error { get; private set; } = [];

    /// <summary>
    /// Gets or sets the FluentValidation result for request validation failures.
    /// </summary>
    public ValidationResult? Validation { get; set; } = new ValidationResult();
    
    /// <summary>
    /// Records an error, marks the result as failed, and appends an entry to <see cref="Error"/>.
    /// </summary>
    /// <param name="errorMessage">The human-readable error description.</param>
    /// <param name="errorType">The category of the error.</param>
    public void SetError(string errorMessage, ErrorType errorType)
    { 
        Error.Add(new Error(errorMessage, errorType));
    }
    
    public static PagedResult<T> Create(){
        return new PagedResult<T>(0, 0, 0, []);
    }

    bool IOperationResult.IsSuccessful => Error.Count == 0 && (Validation?.IsValid ?? true);

    ValidationResult IOperationResult.Validation => Validation ?? new ValidationResult();

    IReadOnlyList<Error> IOperationResult.Errors => Error;
}
