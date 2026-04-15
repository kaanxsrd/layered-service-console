using Project.Data;
using Project.Enums;
using Project.Models;

namespace Project.Services;

/// <summary>
/// Provides authentication services for the application.
/// </summary>
public class AuthService
{
    /// <summary>The currently authenticated user.</summary>
    public User? CurrentUser { get; private set; }

    /// <summary>Check if any user is currently logged in.</summary>
    public bool IsLoggedIn => CurrentUser != null;

    /// <summary>
    /// Authenticates a user by email and password.
    /// </summary>
    /// <param name="email">User email address.</param>
    /// <param name="password">User password.</param>
    /// <returns>True if login is successful, false otherwise.</returns>
    public bool Login(string email, string password)
    {
        var user = UserRepository.GetByEmail(email);
        if (user == null) return false;
        if (user.Password != password) return false;
        if (user.Status != Status.Active) return false;

        CurrentUser = user;
        return true;
    }

    /// <summary>
    /// Logs out the current user.
    /// </summary>
    public void Logout() => CurrentUser = null;
}
