// Metadata descriptor for a pluggable service module, returned by the service registry.
using Project.Domain.Enums;

namespace Project.Domain.Entities;

/// <summary>
/// A projected, read-only view of a registered service module together with its current
/// enabled state. Acts as a transport shape between the registry and the UI layer.
/// </summary>
/// <param name="Id">Unique service ID.</param>
/// <param name="Name">Display name for the service.</param>
/// <param name="Description">Short description of what the service does.</param>
/// <param name="MinimumRole">Minimum role required to execute the service.</param>
/// <param name="IsEnabled">Whether the service is currently enabled.</param>
public sealed record ServiceModule(
    string Id,
    string Name,
    string Description,
    Role   MinimumRole,
    bool   IsEnabled);
