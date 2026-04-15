using Project.Enums;
using Project.Models;
using Project.Services;
using Project.UI;

/// <summary>
/// Entry point for the application.
/// Initializes services and handles the main application loop.
/// </summary>
ServiceRegistry.Register(new ServiceModule
{
    Id          = "word-count",
    Name        = "Word Count",
    Description = "Counts how many times a word appears in a text",
    IsEnabled   = true,
    MinimumRole = Role.User,
    Run         = WordServiceUI.Run,
});

var auth = new AuthService();

if (args.Contains("--dev"))
{
    auth.Login("root@system.local", "root123");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("  [DEV] Auto-logged in as root");
    Console.ResetColor();
    Thread.Sleep(400);
    new Dashboard(auth).Show();
    
}
else if (args.Contains("--user"))
{
    auth.Login("user@system.local", "user123");
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("  [DEV] Auto-logged in as user");
    Console.ResetColor();
    Thread.Sleep(400);
    new Dashboard(auth).Show();
}
else
{
    while (true)
    {
        new LoginScreen(auth).Show();
        new Dashboard(auth).Show();
    }
}
