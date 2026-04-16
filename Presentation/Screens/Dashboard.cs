// Top-level navigation hub shown after login.
using Project.Application.Dtos.Auth;
using Project.Application.Services.Auth;
using Project.Domain.Enums;
using Project.Presentation.Abstractions;

namespace Project.Presentation.Screens;

/// <summary>The main dashboard shown to a logged-in user; routes to sub-screens.</summary>
public sealed class Dashboard : IScreen
{
    private readonly IAuthService             _auth;
    private readonly ServiceRunnerScreen      _runner;
    private readonly ServiceManagementScreen  _serviceMgmt;
    private readonly UserManagementScreen     _userMgmt;

    /// <summary>Creates a new <see cref="Dashboard"/>.</summary>
    public Dashboard(
        IAuthService            auth,
        ServiceRunnerScreen     runner,
        ServiceManagementScreen serviceMgmt,
        UserManagementScreen    userMgmt)
    {
        _auth        = auth;
        _runner      = runner;
        _serviceMgmt = serviceMgmt;
        _userMgmt    = userMgmt;
    }

    /// <inheritdoc />
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            UserResponse? user = _auth.CurrentUser;
            if (user is null) return;

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
            {
                Console.WriteLine("  [2] Manage Services  (Enable / Disable)");
                Console.WriteLine("  [3] Manage Users");
            }
            Console.WriteLine("  [0] Logout");
            Console.WriteLine();
            Console.Write("  Choice: ");

            switch (ConsoleHelper.ReadLine())
            {
                case "1":
                    await _runner.RunAsync(cancellationToken).ConfigureAwait(false);
                    break;

                case "2" when user.Role is Role.Admin or Role.Root:
                    await _serviceMgmt.RunAsync(cancellationToken).ConfigureAwait(false);
                    break;

                case "3" when user.Role is Role.Admin or Role.Root:
                    await _userMgmt.RunAsync(cancellationToken).ConfigureAwait(false);
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

    private static ConsoleColor RoleColor(Role role) => role switch
    {
        Role.Root  => ConsoleColor.Red,
        Role.Admin => ConsoleColor.Yellow,
        _          => ConsoleColor.Green,
    };
}
