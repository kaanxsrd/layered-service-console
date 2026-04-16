// Default IClock implementation wrapping DateTimeOffset.UtcNow.
using Project.Application.Abstractions;

namespace Project.Infrastructure.Time;

/// <summary>Default <see cref="IClock"/> implementation backed by the real system clock.</summary>
public sealed class SystemClock : IClock
{
    /// <inheritdoc />
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

    /// <inheritdoc />
    public DateOnly Today => DateOnly.FromDateTime(DateTime.Today);
}
