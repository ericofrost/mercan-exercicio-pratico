namespace ER.Application.UnitTests.Validators;

public class PaginationValidatorTests
{
    private readonly PaginationValidator _validator = new();

    [Fact]
    public void ValidQuery_ShouldNotHaveValidationErrors()
    {
        var query = new PaginationQueryBuilder().Build();

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void CurrentPageLessThanOne_ShouldHaveValidationError()
    {
        var query = new PaginationQueryBuilder().WithCurrentPage(0).Build();

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.CurrentPage);
    }

    [Fact]
    public void RowsPerPageZero_ShouldHaveValidationError()
    {
        var query = new PaginationQueryBuilder().WithRowsPerPage(0).Build();

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.RowsPerPage);
    }

    [Fact]
    public void RowsPerPageOverMaximum_ShouldHaveValidationError()
    {
        var query = new PaginationQueryBuilder().WithRowsPerPage(101).Build();

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.RowsPerPage);
    }

    [Fact]
    public void EmptyOrderBy_ShouldHaveValidationError()
    {
        var query = new PaginationQueryBuilder().WithOrderBy("").Build();

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.OrderBy);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("")]
    public void InvalidOrder_ShouldHaveValidationError(string order)
    {
        var query = new PaginationQueryBuilder().WithOrder(order).Build();

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.Order);
    }

    [Theory]
    [InlineData("asc")]
    [InlineData("desc")]
    [InlineData("ASC")]
    [InlineData("DESC")]
    public void ValidOrder_ShouldNotHaveValidationError(string order)
    {
        var query = new PaginationQueryBuilder().WithOrder(order).Build();

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(q => q.Order);
    }

    [Fact]
    public void ToRepositoryParameters_ShouldMatchRepositorySignature()
    {
        var query = new PaginationQueryBuilder()
            .WithCurrentPage(2)
            .WithRowsPerPage(15)
            .WithOrderBy("SubmittedAt")
            .WithOrder("desc")
            .Build();

        var (orderBy, order, rowsPerPage, currentPage) = query.ToRepositoryParameters();

        Assert.Equal("SubmittedAt", orderBy);
        Assert.Equal("desc", order);
        Assert.Equal(15, rowsPerPage);
        Assert.Equal(2, currentPage);
    }
}
