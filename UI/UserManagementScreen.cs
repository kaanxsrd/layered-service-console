using Project.Data;
using Project.Enums;
using Project.Models;
using Project.Services;

namespace Project.UI;

/// <summary>
/// Provides a management interface for system users.
/// </summary>
public class UserManagementScreen
{
    private readonly AuthService _auth;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserManagementScreen"/> class.
    /// </summary>
    /// <param name="auth">The authentication service instance.</param>
    public UserManagementScreen(AuthService auth) => _auth = auth;

    /// <summary>
    /// Displays the user management menu and handles user selection.
    /// </summary>
    public void Show()
    {
        while (true)
        {
            ConsoleHelper.WriteHeader("USER MANAGEMENT");

            PrintUserTable();

            Console.WriteLine();
            Console.WriteLine("  [1] Create new user");
            Console.WriteLine("  [2] Toggle user status  (Active / Disabled)");
            Console.WriteLine("  [0] Back");
            Console.WriteLine();
            Console.Write("  Choice: ");

            switch (Console.ReadLine() ?? "")
            {
                case "1": CreateUser();   break;
                case "2": ToggleStatus(); break;
                case "0": return;
                default:
                    ConsoleHelper.WriteError("Invalid choice.");
                    Thread.Sleep(600);
                    break;
            }
        }
    }

    private void PrintUserTable()
    {
        Console.WriteLine($"  {"ID",-5} {"Name",-16} {"Email",-28} {"Role",-8} Status");
        ConsoleHelper.WriteSeparator();

        foreach (var u in UserRepository.GetAll())
        {
            ConsoleColor roleColor = u.Role switch
            {
                Role.Root  => ConsoleColor.Red,
                Role.Admin => ConsoleColor.Yellow,
                _          => ConsoleColor.Green,
            };
            ConsoleColor statusColor = u.Status == Status.Active
                ? ConsoleColor.Green
                : ConsoleColor.DarkRed;

            Console.Write($"  {u.Id,-5}{u.Name,-16}{u.Email,-28}");
            ConsoleHelper.WriteColored($"{u.Role,-8}", roleColor);
            ConsoleHelper.WriteLineColored(u.Status.ToString(), statusColor);
        }
    }

    private void CreateUser()
    {
        ConsoleHelper.WriteHeader("CREATE NEW USER");

        Console.Write("  Name     : ");
        string name = Console.ReadLine() ?? "";

        Console.Write("  Email    : ");
        string email = Console.ReadLine() ?? "";

        if (UserRepository.GetByEmail(email) != null)
        {
            ConsoleHelper.WriteError("Email is already registered.");
            ConsoleHelper.Pause();
            return;
        }

        Console.Write("  Password : ");
        string password = ConsoleHelper.ReadPassword();

        Role role = Role.User;

        if (_auth.CurrentUser!.Role == Role.Root)
        {
            Console.WriteLine("  Role     : [1] User   [2] Admin");
            Console.Write("  Choice   : ");
            role = (Console.ReadLine() ?? "1") == "2" ? Role.Admin : Role.User;
        }

        var user = new User
        {
            Name     = name,
            Email    = email,
            Password = password,
            Role     = role,
            Status   = Status.Active,
        };

        UserRepository.Add(user);
        ConsoleHelper.WriteSuccess($"User '{name}' created. (ID: {user.Id}, Role: {user.Role})");
        ConsoleHelper.Pause();
    }

    private void ToggleStatus()
    {
        Console.Write("  User ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            ConsoleHelper.WriteError("Invalid ID.");
            Thread.Sleep(700);
            return;
        }

        var user = UserRepository.GetById(id);
        if (user == null)
        {
            ConsoleHelper.WriteError("User not found.");
            Thread.Sleep(700);
            return;
        }

        if (user.Role == Role.Root)
        {
            ConsoleHelper.WriteError("The root account cannot be modified.");
            Thread.Sleep(900);
            return;
        }

        if (user.Id == _auth.CurrentUser!.Id)
        {
            ConsoleHelper.WriteError("You cannot disable your own account.");
            Thread.Sleep(900);
            return;
        }

        if (_auth.CurrentUser.Role == Role.Admin && user.Role == Role.Admin)
        {
            ConsoleHelper.WriteError("An admin cannot modify another admin.");
            Thread.Sleep(900);
            return;
        }

        user.Status = user.Status == Status.Active ? Status.Disabled : Status.Active;
        UserRepository.Update(user);

        ConsoleHelper.WriteSuccess($"'{user.Name}' status set to: {user.Status}");
        Thread.Sleep(900);
    }
}
