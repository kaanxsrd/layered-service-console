using Project.Enums;
using Project.Services;

namespace Project.Tests.Services;

public class AuthServiceTests
{
    // Each test gets a fresh AuthService instance — UserRepository seed data is always present.

    [Fact]
    public void Login_WithValidCredentials_ReturnsTrue()
    {
        var auth = new AuthService();
        bool result = auth.Login("root@system.local", "root123");
        Assert.True(result);
    }

    [Fact]
    public void Login_WithValidCredentials_SetsCurrentUser()
    {
        var auth = new AuthService();
        auth.Login("root@system.local", "root123");
        Assert.NotNull(auth.CurrentUser);
        Assert.Equal("Root", auth.CurrentUser.Name);
    }

    [Fact]
    public void Login_WithWrongPassword_ReturnsFalse()
    {
        var auth = new AuthService();
        bool result = auth.Login("root@system.local", "wrongpassword");
        Assert.False(result);
    }

    [Fact]
    public void Login_WithWrongPassword_LeavesCurrentUserNull()
    {
        var auth = new AuthService();
        auth.Login("root@system.local", "wrongpassword");
        Assert.Null(auth.CurrentUser);
    }

    [Fact]
    public void Login_WithUnknownEmail_ReturnsFalse()
    {
        var auth = new AuthService();
        bool result = auth.Login("nobody@example.com", "password");
        Assert.False(result);
    }

    [Theory]
    [InlineData("root@system.local",  "root123",  Role.Root)]
    [InlineData("admin@system.local", "admin123", Role.Admin)]
    [InlineData("user@system.local",  "user123",  Role.User)]
    public void Login_SeedAccounts_HaveCorrectRoles(string email, string password, Role expectedRole)
    {
        var auth = new AuthService();
        auth.Login(email, password);
        Assert.Equal(expectedRole, auth.CurrentUser!.Role);
    }

    [Fact]
    public void Logout_ClearsCurrentUser()
    {
        var auth = new AuthService();
        auth.Login("root@system.local", "root123");
        auth.Logout();
        Assert.Null(auth.CurrentUser);
    }

    [Fact]
    public void IsLoggedIn_IsFalse_BeforeLogin()
    {
        var auth = new AuthService();
        Assert.False(auth.IsLoggedIn);
    }

    [Fact]
    public void IsLoggedIn_IsTrue_AfterSuccessfulLogin()
    {
        var auth = new AuthService();
        auth.Login("root@system.local", "root123");
        Assert.True(auth.IsLoggedIn);
    }

    [Fact]
    public void IsLoggedIn_IsFalse_AfterLogout()
    {
        var auth = new AuthService();
        auth.Login("root@system.local", "root123");
        auth.Logout();
        Assert.False(auth.IsLoggedIn);
    }
}
