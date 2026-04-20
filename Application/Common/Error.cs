// Value type carrying a machine-readable code and a human-readable message for a failure.
namespace Project.Application.Common;

/// <summary>
/// Represents a failure returned from the application layer. <see cref="Code"/> is a short,
/// stable identifier suitable for switches or telemetry; <see cref="Message"/> is display text.
/// </summary>
/// <param name="Code">Stable, machine-readable error code. See <see cref="Errors.Codes"/>.</param>
/// <param name="Message">Human-readable, display-safe message describing the failure.</param>
public readonly record struct Error(string Code, string Message)
{
    /// <summary>A sentinel representing the absence of an error.</summary>
    public static Error None { get; } = new(string.Empty, string.Empty);

    /// <summary>True when this error represents no failure (equal to <see cref="None"/>).</summary>
    public bool IsNone => string.IsNullOrEmpty(Code);
}
