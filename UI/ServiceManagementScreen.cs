using Project.Services;

namespace Project.UI;

/// <summary>
/// Provides a management interface for enabling and disabling services.
/// </summary>
public class ServiceManagementScreen
{
    /// <summary>
    /// Displays the service management menu and handles user actions.
    /// </summary>
    public void Show()
    {
        while (true)
        {
            ConsoleHelper.WriteHeader("SERVICE MANAGEMENT  —  Enable / Disable");

            var services = ServiceRegistry.GetAll();

            if (services.Count == 0)
            {
                ConsoleHelper.WriteWarning("No services registered.");
                ConsoleHelper.Pause();
                return;
            }

            Console.WriteLine($"  {"#",-4} {"Status",-10} {"Service Name",-26} Description");
            ConsoleHelper.WriteSeparator();

            for (int i = 0; i < services.Count; i++)
            {
                var svc = services[i];
                bool on = svc.IsEnabled;
                string statusLabel = on ? "[ ON  ]" : "[ OFF ]";
                ConsoleColor statusColor = on ? ConsoleColor.Green : ConsoleColor.Red;

                Console.Write($"  [{i + 1}] ");
                ConsoleHelper.WriteColored($"{statusLabel,-10}", statusColor);
                Console.Write($" {svc.Name,-26}");
                ConsoleHelper.WriteLineColored(svc.Description, ConsoleColor.DarkGray);
            }

            Console.WriteLine();
            Console.WriteLine("  [0] Back");
            Console.WriteLine();
            Console.Write("  Enter service number to toggle: ");
            string input = Console.ReadLine() ?? "";

            if (input == "0") return;

            if (int.TryParse(input, out int idx) && idx >= 1 && idx <= services.Count)
            {
                var svc = services[idx - 1];
                if (svc.IsEnabled)
                {
                    ServiceRegistry.Disable(svc.Id);
                    ConsoleHelper.WriteSuccess($"'{svc.Name}' disabled.");
                }
                else
                {
                    ServiceRegistry.Enable(svc.Id);
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
    }
}
