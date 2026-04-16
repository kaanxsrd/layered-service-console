// Login screen: prompts for credentials until authentication succeeds.
using Project.Application.Dtos.Auth;
using Project.Application.Services.Auth;
using Project.Presentation.Abstractions;
using Project.Presentation.Rendering;

namespace Project.Presentation.Screens;

/// <summary>Interactive login flow; loops until the user authenticates successfully.</summary>
public sealed class LoginScreen : IScreen
{
    private readonly IAuthService _auth;

    /// <summary>Creates a new <see cref="LoginScreen"/>.</summary>
    public LoginScreen(IAuthService auth) => _auth = auth;

    /// <inheritdoc />
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
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
            string email = ConsoleHelper.ReadLine();

            Console.Write("  Password : ");
            string password = ConsoleHelper.ReadPassword();

            Console.WriteLine();

            var result = await _auth.LoginAsync(new LoginRequest(email, password), cancellationToken)
                .ConfigureAwait(false);

            if (result.IsSuccess)
            {
                var user = result.Value.User;
                ConsoleHelper.WriteSuccess($"Welcome, {user.Name}! ({user.Role})");
                await Task.Delay(900, cancellationToken).ConfigureAwait(false);
                return;
            }

            ResultRenderer.RenderError(result.Error!.Value);
            ConsoleHelper.Pause("  Press Enter to try again...");
        }
    }
}
