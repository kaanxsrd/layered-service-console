using Project.Enums;
using Project.Models;

namespace Project.Services;

/// <summary>
/// Manages registration and availability of system services.
/// </summary>
public static class ServiceRegistry
{
    private static readonly List<ServiceModule> _services = new();

    /// <summary>
    /// Registers a new service module.
    /// </summary>
    /// <param name="service">The service module to register.</param>
    public static void Register(ServiceModule service) => _services.Add(service);

    /// <summary>
    /// Gets all registered services.
    /// </summary>
    /// <returns>List of all registered service modules.</returns>
    public static List<ServiceModule> GetAll() => _services.ToList();

    /// <summary>
    /// Gets services available for a specific user role.
    /// </summary>
    /// <param name="userRole">The role of the user.</param>
    /// <returns>List of enabled services the user has access to.</returns>
    public static List<ServiceModule> GetAvailableFor(Role userRole) =>
        _services
            .Where(s => s.IsEnabled && (int)userRole <= (int)s.MinimumRole)
            .ToList();

    /// <summary>
    /// Enables a service by ID.
    /// </summary>
    /// <param name="id">The service identifier.</param>
    public static void Enable(string id)
    {
        var svc = _services.FirstOrDefault(s => s.Id == id);
        if (svc != null) svc.IsEnabled = true;
    }

    /// <summary>
    /// Disables a service by ID.
    /// </summary>
    /// <param name="id">The service identifier.</param>
    public static void Disable(string id)
    {
        var svc = _services.FirstOrDefault(s => s.Id == id);
        if (svc != null) svc.IsEnabled = false;
    }

    /// <summary>
    /// Clears all registered services.
    /// </summary>
    public static void Reset() => _services.Clear();
}
