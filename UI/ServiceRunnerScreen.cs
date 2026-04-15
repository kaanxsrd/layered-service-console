using Project.Services;

namespace Project.UI;

/// <summary>
/// Provides a selection screen for running available services.
/// </summary>
public class ServiceRunnerScreen
{
    private readonly AuthService _auth;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRunnerScreen"/> class.
    /// </summary>
    /// <param name="auth">The authentication service instance.</param>
    public ServiceRunnerScreen(AuthService auth) => _auth = auth;

    /// <summary>
    /// Displays the service selection menu and handles execution.
    /// </summary>
    public void Show()
    {
        while (true)
        {
            ConsoleHelper.WriteHeader("USE SERVICES");

            var available = ServiceRegistry.GetAvailableFor(_auth.CurrentUser!.Role);

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

            string input = Console.ReadLine() ?? "";
            if (input == "0") return;

            if (int.TryParse(input, out int idx) && idx >= 1 && idx <= available.Count)
            {
                available[idx - 1].Run();
            }
            else
            {
                ConsoleHelper.WriteError("Invalid choice.");
                Thread.Sleep(600);
            }
        }
    }
}
