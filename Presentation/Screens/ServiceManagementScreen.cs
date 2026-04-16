// Toggle enable/disable state for registered service modules.
using Project.Application.Abstractions;
using Project.Presentation.Abstractions;

namespace Project.Presentation.Screens;

/// <summary>Management screen that lets admins enable or disable service modules.</summary>
public sealed class ServiceManagementScreen : IScreen
{
    private readonly IServiceRegistry _registry;

    /// <summary>Creates a new <see cref="ServiceManagementScreen"/>.</summary>
    public ServiceManagementScreen(IServiceRegistry registry) => _registry = registry;

    /// <inheritdoc />
    public Task RunAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            ConsoleHelper.WriteHeader("SERVICE MANAGEMENT  —  Enable / Disable");

            var services = _registry.GetAll();

            if (services.Count == 0)
            {
                ConsoleHelper.WriteWarning("No services registered.");
                ConsoleHelper.Pause();
                return Task.CompletedTask;
            }

            Console.WriteLine($"  {"#",-4} {"Status",-10} {"Service Name",-26} Description");
            ConsoleHelper.WriteSeparator();

            for (int i = 0; i < services.Count; i++)
            {
                var svc = services[i];
                string statusLabel = svc.IsEnabled ? "[ ON  ]" : "[ OFF ]";
                var    statusColor = svc.IsEnabled ? ConsoleColor.Green : ConsoleColor.Red;

                Console.Write($"  [{i + 1}] ");
                ConsoleHelper.WriteColored($"{statusLabel,-10}", statusColor);
                Console.Write($" {svc.Name,-26}");
                ConsoleHelper.WriteLineColored(svc.Description, ConsoleColor.DarkGray);
            }

            Console.WriteLine();
            Console.WriteLine("  [0] Back");
            Console.WriteLine();
            Console.Write("  Enter service number to toggle: ");
            string input = ConsoleHelper.ReadLine();

            if (input == "0") return Task.CompletedTask;

            if (int.TryParse(input, out int idx) && idx >= 1 && idx <= services.Count)
            {
                var svc = services[idx - 1];
                if (svc.IsEnabled)
                {
                    _registry.Disable(svc.Id);
                    ConsoleHelper.WriteSuccess($"'{svc.Name}' disabled.");
                }
                else
                {
                    _registry.Enable(svc.Id);
                    ConsoleHelper.WriteSuccess($"'{svc.Name}' enabled.");
                }
                Thread.Sleep(700);
            }
            else
            {
                ConsoleHelper.WriteError("Invalid choice.");
                Thread.Sleep(600);
            }
        }

        return Task.CompletedTask;
    }
}
