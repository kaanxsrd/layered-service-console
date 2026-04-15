using Project.Enums;

namespace Project.Models;

/// <summary>
/// A module representing a pluggable service within the application.
/// </summary>
public class ServiceModule
{
    /// <summary>Unique service ID.</summary>
    public string Id { get; set; } = string.Empty;
    /// <summary>Display name for the service.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>A brief description of what the service does.</summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>Whether the service is currently enabled.</summary>
    public bool IsEnabled { get; set; }
    /// <summary>Minimum role required to execute this service.</summary>
    public Role MinimumRole { get; set; } = Role.User;
    /// <summary>The execution logic for this service.</summary>
    public Action Run { get; set; } = () => { };
}
