using Project.Enums;
using Project.Models;
using Project.Services;

namespace Project.UI;

/// <summary>
/// The main dashboard screen for logged-in users.
/// </summary>
public class Dashboard
{
    private readonly AuthService _auth;
    private readonly ServiceManagementScreen _svcManagement = new();
    private readonly UserManagementScreen _userManagement;
    private readonly ServiceRunnerScreen _svcRunner;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dashboard"/> class.
    /// </summary>
    /// <param name="auth">The authentication service instance.</param>
    public Dashboard(AuthService auth)
    {
        _auth           = auth;
        _userManagement = new UserManagementScreen(auth);
        _svcRunner      = new ServiceRunnerScreen(auth);
    }

    /// <summary>
    /// Displays the dashboard menu and handles user navigation.
    /// </summary>
    public void Show()
    {
        while (true)
        {
            User user = _auth.CurrentUser!;

            ConsoleHelper.WriteHeader("DASHBOARD");

            Console.Write("  User  : ");
            Console.WriteLine(user.Name);

            Console.Write("  Role  : ");
            ConsoleHelper.WriteLineColored(user.Role.ToString(), RoleColor(user.Role));

            Console.Write("  Email : ");
            ConsoleHelper.WriteLineColored(user.Email, ConsoleColor.DarkGray);

            Console.WriteLine();
            ConsoleHelper.WriteSeparator();
            Console.WriteLine();

            Console.WriteLine("  [1] Use Services");

            if (user.Role is Role.Admin or Role.Root)
                Console.WriteLine("  [2] Manage Services  (Enable / Disable)");

            if (user.Role is Role.Admin or Role.Root)
                Console.WriteLine("  [3] Manage Users");

            Console.WriteLine("  [0] Logout");
            Console.WriteLine();
            Console.Write("  Choice: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    _svcRunner.Show();
                    break;

                case "2" when user.Role is Role.Admin or Role.Root:
                    _svcManagement.Show();
                    break;

                case "3" when user.Role is Role.Admin or Role.Root:
                    _userManagement.Show();
                    break;

                case "0":
                    _auth.Logout();
                    return;

                default:
                    ConsoleHelper.WriteError("Invalid choice.");
                    Thread.Sleep(600);
                    break;
            }
        }
    }

    /// <summary>
    /// Returns the console color associated with a user role.
    /// </summary>
    /// <param name="role">The user role.</param>
    /// <returns>A <see cref="ConsoleColor"/> value.</returns>
    private static ConsoleColor RoleColor(Role role) => role switch
    {
        Role.Root  => ConsoleColor.Red,
        Role.Admin => ConsoleColor.Yellow,
        _          => ConsoleColor.Green,
    };
}
