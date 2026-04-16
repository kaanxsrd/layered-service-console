// Lists service modules available to the current user and runs the chosen one.
using Project.Application.Abstractions;
using Project.Application.Services.Auth;
using Project.Presentation.Abstractions;
using Project.Presentation.Rendering;

namespace Project.Presentation.Screens;

/// <summary>Selection screen that runs the service module the user picks.</summary>
public sealed class ServiceRunnerScreen : IScreen
{
    private readonly IAuthService     _auth;
    private readonly IServiceRegistry _registry;

    /// <summary>Creates a new <see cref="ServiceRunnerScreen"/>.</summary>
    public ServiceRunnerScreen(IAuthService auth, IServiceRegistry registry)
    {
        _auth     = auth;
        _registry = registry;
    }

    /// <inheritdoc />
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            ConsoleHelper.WriteHeader("USE SERVICES");

            if (_auth.CurrentUser is null)
            {
                ConsoleHelper.WriteError("Not logged in.");
                ConsoleHelper.Pause();
                return;
            }

            var available = _registry.GetAvailableFor(_auth.CurrentUser.Role);

            if (available.Count == 0)
            {
                ConsoleHelper.WriteWarning("No services available.");
                ConsoleHelper.Pause();
                return;
            }

            for (int i = 0; i < available.Count; i++)
            {
                var svc = available[i];
                Console.Write($"  [{i + 1}] ");
                ConsoleHelper.WriteColored($"{svc.Name,-28}", ConsoleColor.Cyan);
                ConsoleHelper.WriteLineColored(svc.Description, ConsoleColor.DarkGray);
            }

            Console.WriteLine();
            Console.WriteLine("  [0] Back");
            Console.WriteLine();
            Console.Write("  Select service: ");

            string input = ConsoleHelper.ReadLine();
            if (input == "0") return;

            if (int.TryParse(input, out int idx) && idx >= 1 && idx <= available.Count)
            {
                var chosen = available[idx - 1];
                var result = await _registry
                    .RunAsync(chosen.Id, _auth.CurrentUser.Role, cancellationToken)
                    .ConfigureAwait(false);

                if (result.IsFailure)
                {
                    ResultRenderer.RenderError(result.Error!.Value);
                    ConsoleHelper.Pause();
                }
            }
            else
            {
                ConsoleHelper.WriteError("Invalid choice.");
                Thread.Sleep(600);
            }
        }
    }
}
