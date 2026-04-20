using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Project.Application.Abstractions;
using Project.Application.Dtos.Auth;
using Project.Application.Services.Auth;
using Project.Application.Services.Files;
using Project.Application.Services.Modules;
using Project.Application.Services.Pension;
using Project.Application.Services.Text;
using Project.Infrastructure.Auth;
using Project.Infrastructure.Configuration;
using Project.Infrastructure.Persistence;
using Project.Infrastructure.Security;
using Project.Infrastructure.Time;
using Project.Presentation;
using Project.Presentation.Modules;
using Project.Presentation.Rendering;
using Project.Presentation.Screens;

var services = new ServiceCollection();

services.AddLogging(builder =>
{
    builder
        .AddSimpleConsole(options =>
        {
            options.SingleLine       = true;
            options.TimestampFormat  = "HH:mm:ss ";
            options.IncludeScopes    = false;
        })
        .SetMinimumLevel(LogLevel.Warning);
});

services.AddSingleton<IConfigurationService>(_ =>
    new JsonConfigurationService(Path.Combine(AppContext.BaseDirectory, "appsettings.json")));

services.AddSingleton<IClock,          SystemClock>();
services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
services.AddSingleton<ITokenService,   StubTokenService>();

// TODO(db): swap InMemoryUserRepository for SqlUserRepository once the database is provisioned
services.AddSingleton<IUserRepository, InMemoryUserRepository>();

services.AddSingleton<IAuthService,     AuthService>();
services.AddSingleton<IFileService,     FileService>();
services.AddSingleton<IServiceRegistry, ServiceRegistry>();

services.AddSingleton<PensionCalculator>();
services.AddSingleton<WordService>();

services.AddSingleton<PensionServiceUI>();
services.AddSingleton<WordServiceUI>();
services.AddSingleton<LoginScreen>();
services.AddSingleton<Dashboard>();
services.AddSingleton<ServiceRunnerScreen>();
services.AddSingleton<ServiceManagementScreen>();
services.AddSingleton<UserManagementScreen>();

services.AddSingleton<IServiceModule, PensionServiceModule>();
services.AddSingleton<IServiceModule, WordServiceModule>();

await using var provider = services.BuildServiceProvider();

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

var logger = provider.GetRequiredService<ILogger<Program>>();

try
{
    var auth      = provider.GetRequiredService<IAuthService>();
    var login     = provider.GetRequiredService<LoginScreen>();
    var dashboard = provider.GetRequiredService<Dashboard>();

    if (args.Contains("--dev"))
    {
        await AutoLoginAsync(auth, "root@system.local", "root123", "root", cts.Token).ConfigureAwait(false);
        await dashboard.RunAsync(cts.Token).ConfigureAwait(false);
        return 0;
    }

    if (args.Contains("--user"))
    {
        await AutoLoginAsync(auth, "user@system.local", "user123", "user", cts.Token).ConfigureAwait(false);
        await dashboard.RunAsync(cts.Token).ConfigureAwait(false);
        return 0;
    }

    while (!cts.IsCancellationRequested)
    {
        await login.RunAsync(cts.Token).ConfigureAwait(false);
        await dashboard.RunAsync(cts.Token).ConfigureAwait(false);
    }

    return 0;
}
catch (OperationCanceledException)
{
    return 0;
}
catch (Exception ex)
{
    logger.LogError(ex, "Fatal unhandled exception");
    ResultRenderer.RenderError(new Project.Application.Common.Error("Fatal", ex.Message));
    return 1;
}

static async Task AutoLoginAsync(IAuthService auth, string email, string password, string label, CancellationToken ct)
{
    var result = await auth.LoginAsync(new LoginRequest(email, password), ct).ConfigureAwait(false);
    if (result.IsFailure)
    {
        ResultRenderer.RenderError(result.Error!.Value);
        return;
    }
    ConsoleHelper.WriteWarning($"[DEV] Auto-logged in as {label}");
    await Task.Delay(400, ct).ConfigureAwait(false);
}
