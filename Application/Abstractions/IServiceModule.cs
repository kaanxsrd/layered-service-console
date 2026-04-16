// Contract implemented by every pluggable service module. Resolved via DI.
using Project.Domain.Enums;

namespace Project.Application.Abstractions;

/// <summary>
/// Contract for a pluggable service module. Modules are resolved from the container as
/// <c>IEnumerable&lt;IServiceModule&gt;</c> and surfaced through
/// <see cref="IServiceRegistry"/> for listing and execution.
/// </summary>
public interface IServiceModule
{
    /// <summary>Stable, unique module identifier.</summary>
    string Id { get; }

    /// <summary>Human-readable display name.</summary>
    string Name { get; }

    /// <summary>Short description of what the module does.</summary>
    string Description { get; }

    /// <summary>Minimum role required to run this module.</summary>
    Role MinimumRole { get; }

    /// <summary>Executes the module's interactive flow.</summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task RunAsync(CancellationToken cancellationToken);
}
