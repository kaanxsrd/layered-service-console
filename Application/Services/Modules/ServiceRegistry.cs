// DI-driven registry: modules are supplied via IEnumerable<IServiceModule>, state tracked internally.
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Project.Application.Abstractions;
using Project.Application.Common;
using Project.Domain.Entities;
using Project.Domain.Enums;

namespace Project.Application.Services.Modules;

/// <summary>
/// Default <see cref="IServiceRegistry"/>: enumerates modules from DI and tracks each
/// module's enabled state in a concurrent map. Modules start enabled by default.
/// </summary>
public sealed class ServiceRegistry : IServiceRegistry
{
    private readonly IReadOnlyDictionary<string, IServiceModule> _modules;
    private readonly ConcurrentDictionary<string, bool>          _enabled;
    private readonly ILogger<ServiceRegistry>                    _logger;

    /// <summary>Creates a registry from the DI-supplied set of modules.</summary>
    public ServiceRegistry(IEnumerable<IServiceModule> modules, ILogger<ServiceRegistry> logger)
    {
        _modules = modules.ToDictionary(m => m.Id, StringComparer.Ordinal);
        _enabled = new ConcurrentDictionary<string, bool>(
            _modules.Keys.Select(id => new KeyValuePair<string, bool>(id, true)));
        _logger  = logger;
    }

    /// <inheritdoc />
    public IReadOnlyList<ServiceModule> GetAll() =>
        _modules.Values.Select(Project).ToList();

    /// <inheritdoc />
    public IReadOnlyList<ServiceModule> GetAvailableFor(Role role) =>
        _modules.Values
            .Where(m => IsEnabled(m.Id) && (int)role <= (int)m.MinimumRole)
            .Select(Project)
            .ToList();

    /// <inheritdoc />
    public void Enable(string id)
    {
        if (_modules.ContainsKey(id))
            _enabled[id] = true;
    }

    /// <inheritdoc />
    public void Disable(string id)
    {
        if (_modules.ContainsKey(id))
            _enabled[id] = false;
    }

    /// <inheritdoc />
    public async Task<Result> RunAsync(string id, Role role, CancellationToken cancellationToken)
    {
        if (!_modules.TryGetValue(id, out var module))
            return Result.Failure(Errors.NotFound($"Service '{id}' not found."));

        if (!IsEnabled(id))
            return Result.Failure(Errors.Conflict($"Service '{id}' is disabled."));

        if ((int)role > (int)module.MinimumRole)
            return Result.Failure(Errors.Unauthorized($"Role '{role}' cannot run '{module.Name}'."));

        _logger.LogInformation("Running module {ModuleId}", id);
        await module.RunAsync(cancellationToken).ConfigureAwait(false);
        return Result.Success();
    }

    private bool IsEnabled(string id) => _enabled.TryGetValue(id, out var on) && on;

    private ServiceModule Project(IServiceModule module) =>
        new(module.Id, module.Name, module.Description, module.MinimumRole, IsEnabled(module.Id));
}
