// Abstraction over the system clock; lets services avoid direct DateTime.UtcNow calls.
namespace Project.Application.Abstractions;

/// <summary>
/// Abstraction over the system clock. Services must depend on this rather than calling
/// <see cref="System.DateTime.UtcNow"/> or <see cref="System.DateTime.Now"/> directly so
/// time-dependent behavior can be controlled in tests.
/// </summary>
public interface IClock
{
    /// <summary>The current UTC time.</summary>
    DateTimeOffset UtcNow { get; }

    /// <summary>The current local date in the system's time zone.</summary>
    DateOnly Today { get; }
}
