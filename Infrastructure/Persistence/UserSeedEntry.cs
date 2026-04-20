using Project.Domain.Enums;

namespace Project.Infrastructure.Persistence;

/// <summary>
/// Represents a single user entry as stored in the <c>Users</c> section of
/// <c>appsettings.json</c>. <see cref="Password"/> is plain text — it is hashed
/// by <see cref="InMemoryUserRepository"/> at startup and never stored in memory.
/// </summary>
public sealed record UserSeedEntry(
    Guid   Id,
    string Name,
    string Email,
    string Password,
    Role   Role,
    Status Status
);
