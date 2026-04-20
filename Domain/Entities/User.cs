// Aggregate representing an authenticated principal.
using Project.Domain.Enums;
using Project.Domain.ValueObjects;

namespace Project.Domain.Entities;

/// <summary>
/// Represents a user entity in the system. Credentials are stored as a hash; never plaintext.
/// </summary>
public sealed class User
{
    /// <summary>Unique identifier for the user.</summary>
    public Guid Id { get; init; }

    /// <summary>Display name of the user.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Primary email address for the user.</summary>
    public Email Email { get; set; } = Email.From(string.Empty);

    /// <summary>Hashed password and salt used to authenticate the user.</summary>
    public PasswordHash PasswordHash { get; set; } = PasswordHash.Empty;

    /// <summary>Access role assigned to the user.</summary>
    public Role Role { get; set; }

    /// <summary>Current account status of the user.</summary>
    public Status Status { get; set; }
}
