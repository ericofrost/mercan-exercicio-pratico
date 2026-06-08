namespace ER.Application.UnitTests.Mocks;

public static class HttpContextMockConfigurator
{
    public static IHttpContextAccessor CreateAccessorWithPrincipal(ClaimsPrincipal? principal)
    {
        var httpContext = new DefaultHttpContext();

        if (principal is not null)
        {
            httpContext.User = principal;
        }

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);
        return accessor.Object;
    }

    public static IHttpContextAccessor CreateAccessorWithoutContext()
    {
        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns((HttpContext?)null);
        return accessor.Object;
    }
}
