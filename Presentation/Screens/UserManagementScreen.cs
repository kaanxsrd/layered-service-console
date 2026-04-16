// User management screen: create users and toggle their status via IAuthService.
using Project.Application.Dtos.Auth;
using Project.Application.Services.Auth;
using Project.Domain.Enums;
using Project.Presentation.Abstractions;
using Project.Presentation.Rendering;

namespace Project.Presentation.Screens;

/// <summary>Admin-facing screen for creating users and toggling account status.</summary>
public sealed class UserManagementScreen : IScreen
{
    private readonly IAuthService _auth;

    /// <summary>Creates a new <see cref="UserManagementScreen"/>.</summary>
    public UserManagementScreen(IAuthService auth) => _auth = auth;

    /// <inheritdoc />
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            ConsoleHelper.WriteHeader("USER MANAGEMENT");

            await PrintUserTableAsync(cancellationToken).ConfigureAwait(false);

            Console.WriteLine();
            Console.WriteLine("  [1] Create new user");
            Console.WriteLine("  [2] Toggle user status  (Active / Disabled)");
            Console.WriteLine("  [0] Back");
            Console.WriteLine();
            Console.Write("  Choice: ");

            switch (ConsoleHelper.ReadLine())
            {
                case "1":
                    await CreateUserAsync(cancellationToken).ConfigureAwait(false);
                    break;
                case "2":
                    await ToggleStatusAsync(cancellationToken).ConfigureAwait(false);
                    break;
                case "0":
                    return;
                default:
                    ConsoleHelper.WriteError("Invalid choice.");
                    Thread.Sleep(600);
                    break;
            }
        }
    }

    private async Task PrintUserTableAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine($"  {"Name",-16} {"Email",-28} {"Role",-8} Status");
        ConsoleHelper.WriteSeparator();

        var users = await _auth.ListUsersAsync(cancellationToken).ConfigureAwait(false);

        foreach (var u in users)
        {
            var roleColor = u.Role switch
            {
                Role.Root  => ConsoleColor.Red,
                Role.Admin => ConsoleColor.Yellow,
                _          => ConsoleColor.Green,
            };
            var statusColor = u.Status == Status.Active
                ? ConsoleColor.Green
                : ConsoleColor.DarkRed;

            Console.Write($"  {u.Name,-16}{u.Email,-28}");
            ConsoleHelper.WriteColored($"{u.Role,-8}", roleColor);
            ConsoleHelper.WriteLineColored(u.Status.ToString(), statusColor);
        }
    }

    private async Task CreateUserAsync(CancellationToken cancellationToken)
    {
        ConsoleHelper.WriteHeader("CREATE NEW USER");

        string name  = ConsoleHelper.ReadNonEmpty("  Name     : ");
        string email = ConsoleHelper.ReadNonEmpty("  Email    : ");

        Console.Write("  Password : ");
        string password = ConsoleHelper.ReadPassword();

        Role role = Role.User;
        if (_auth.CurrentUser?.Role == Role.Root)
        {
            Console.WriteLine("  Role     : [1] User   [2] Admin");
            Console.Write("  Choice   : ");
            role = ConsoleHelper.ReadLine() == "2" ? Role.Admin : Role.User;
        }

        var request = new RegisterUserRequest(name, email, password, role);
        var result  = await _auth.RegisterAsync(request, cancellationToken).ConfigureAwait(false);

        ResultRenderer.Render(result, u => $"User '{u.Name}' created. (Role: {u.Role})");
        ConsoleHelper.Pause();
    }

    private async Task ToggleStatusAsync(CancellationToken cancellationToken)
    {
        Console.Write("  User email: ");
        string email = ConsoleHelper.ReadLine();

        var users  = await _auth.ListUsersAsync(cancellationToken).ConfigureAwait(false);
        var target = users.FirstOrDefault(
            u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (target is null)
        {
            ConsoleHelper.WriteError("User not found.");
            Thread.Sleep(700);
            return;
        }

        var next   = target.Status == Status.Active ? Status.Disabled : Status.Active;
        var result = await _auth.SetStatusAsync(target.Id, next, cancellationToken).ConfigureAwait(false);

        ResultRenderer.Render(result, u => $"'{u.Name}' status set to: {u.Status}");
        Thread.Sleep(900);
    }
}
