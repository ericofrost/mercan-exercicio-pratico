namespace ER.Application.UnitTests.Builders;

public sealed class PaginationQueryBuilder
{
    private int _page = 1;
    private int _pageSize = 20;

    public PaginationQueryBuilder WithPage(int page)
    {
        _page = page;
        return this;
    }

    public PaginationQueryBuilder WithPageSize(int pageSize)
    {
        _pageSize = pageSize;
        return this;
    }

    public PaginationQuery Build() => new(_page, _pageSize);
}
