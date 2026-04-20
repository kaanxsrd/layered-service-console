// Abstraction for the runtime service-module registry and dispatcher.
using Project.Application.Common;
using Project.Domain.Entities;
using Project.Domain.Enums;

namespace Project.Application.Abstractions;

/// <summary>
/// Runtime registry of <see cref="IServiceModule"/>s. Tracks per-module enabled state,
/// filters modules by role, and dispatches execution by id.
/// </summary>
public interface IServiceRegistry
{
    /// <summary>Returns every registered module with its current enabled state.</summary>
    IReadOnlyList<ServiceModule> GetAll();

    /// <summary>Returns enabled modules the given role can access.</summary>
    /// <param name="role">The caller's role.</param>
    IReadOnlyList<ServiceModule> GetAvailableFor(Role role);

    /// <summary>Enables the module with the given id. No-op when unknown.</summary>
    /// <param name="id">Module identifier.</param>
    void Enable(string id);

    /// <summary>Disables the module with the given id. No-op when unknown.</summary>
    /// <param name="id">Module identifier.</param>
    void Disable(string id);

    /// <summary>
    /// Executes the module with the given id, returning a failure result if the module
    /// is unknown or is not permitted for <paramref name="role"/>.
    /// </summary>
    /// <param name="id">Module identifier.</param>
    /// <param name="role">The caller's role, used for authorization.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<Result> RunAsync(string id, Role role, CancellationToken cancellationToken);
}
