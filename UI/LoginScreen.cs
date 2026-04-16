using Project.Models;
using Project.Services;
using System.Runtime.InteropServices.JavaScript;
namespace Project.UI;

/// <summary>
/// Provides a login screen for user authentication.
/// </summary>
public class LoginScreen
{
    private readonly AuthService _auth;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginScreen"/> class.
    /// </summary>
    /// <param name="auth">The authentication service instance.</param>
    public LoginScreen(AuthService auth) => _auth = auth;

    /// <summary>
    /// Displays the login screen and handles user authentication.
    /// </summary>
    /// <returns>The authenticated <see cref="User"/> object.</returns>
    public User Show()
    {
        while (true)
        {
            ConsoleHelper.WriteHeader("PROJECT PANEL  —  LOGIN");

            ConsoleHelper.WriteInfo("Default accounts:");
            Console.WriteLine("    root@system.local   /  root123   (Root)");
            Console.WriteLine("    admin@system.local  /  admin123  (Admin)");
            Console.WriteLine("    user@system.local   /  user123   (User)");
            Console.WriteLine();
            ConsoleHelper.WriteSeparator();
            Console.WriteLine();

            Console.Write("  Email    : ");
            string email = Console.ReadLine() ?? string.Empty;

            Console.Write("  Password : ");
            string password = ConsoleHelper.ReadPassword();

            Console.WriteLine();

            if (_auth.Login(email, password))
            {
                ConsoleHelper.WriteSuccess($"Welcome, {_auth.CurrentUser!.Name}! ({_auth.CurrentUser.Role})");
                Thread.Sleep(900);
                return _auth.CurrentUser;
            }

            ConsoleHelper.WriteError("Invalid email or password, or account is not active.");
            ConsoleHelper.Pause("  Press Enter to try again...");
        }
    }
}
