namespace ER.Application.UnitTests.Mocks;

public static class UserManagerMockConfigurator
{
    public static Mock<UserManager<ApplicationUser>> Create()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        return new Mock<UserManager<ApplicationUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    public static void SetupUsersQuery(Mock<UserManager<ApplicationUser>> userManager, IEnumerable<ApplicationUser> users)
    {
        userManager.Setup(m => m.Users).Returns(users.AsAsyncQueryable());
    }

    public static void SetupUsersQueryThrows(Mock<UserManager<ApplicationUser>> userManager, Exception exception)
    {
        userManager.Setup(m => m.Users).Throws(exception);
    }

    public static void SetupCheckPasswordAsync(Mock<UserManager<ApplicationUser>> userManager, ApplicationUser user, string password, bool result)
    {
        userManager
            .Setup(m => m.CheckPasswordAsync(user, password))
            .ReturnsAsync(result);
    }

    public static void SetupCreateAsyncSuccess(Mock<UserManager<ApplicationUser>> userManager)
    {
        userManager
            .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
    }

    public static void SetupCreateAsyncFailure(Mock<UserManager<ApplicationUser>> userManager, params string[] errorCodes)
    {
        var errors = errorCodes.Select(code => new IdentityError { Code = code, Description = "error" });
        userManager
            .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(errors.ToArray()));
    }
}
