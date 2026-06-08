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
    public void PageLessThanOne_ShouldHaveValidationError()
    {
        var query = new PaginationQueryBuilder().WithPage(0).Build();

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.Page);
    }

    [Fact]
    public void PageSizeZero_ShouldHaveValidationError()
    {
        var query = new PaginationQueryBuilder().WithPageSize(0).Build();

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.PageSize);
    }

    [Fact]
    public void PageSizeOverMaximum_ShouldHaveValidationError()
    {
        var query = new PaginationQueryBuilder().WithPageSize(101).Build();

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.PageSize);
    }
}
