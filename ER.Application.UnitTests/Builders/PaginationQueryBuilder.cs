namespace ER.Application.UnitTests.Builders;

public sealed class PaginationQueryBuilder
{
    private int _currentPage = 1;
    private int _rowsPerPage = 20;
    private string _orderBy = "Id";
    private string _order = "asc";

    public PaginationQueryBuilder WithCurrentPage(int currentPage)
    {
        _currentPage = currentPage;
        return this;
    }

    public PaginationQueryBuilder WithRowsPerPage(int rowsPerPage)
    {
        _rowsPerPage = rowsPerPage;
        return this;
    }

    public PaginationQueryBuilder WithOrderBy(string orderBy)
    {
        _orderBy = orderBy;
        return this;
    }

    public PaginationQueryBuilder WithOrder(string order)
    {
        _order = order;
        return this;
    }

    public PaginationRequestDto Build() => new(_currentPage, _rowsPerPage, _orderBy, _order);
}
