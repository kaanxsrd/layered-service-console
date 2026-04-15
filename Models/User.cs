using Project.Enums;

namespace Project.Models;

/// <summary>
/// Represents a user entity in the system.
/// </summary>
public class User
{
    /// <summary>Unique identifier for the user.</summary>
    public int Id { get; set; }
    /// <summary>Display name of the user.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Primary email address for the user.</summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>User password for authentication.</summary>
    public string Password { get; set; } = string.Empty;
    /// <summary>Access role assigned to the user.</summary>
    public Role Role { get; set; }
    /// <summary>Current account status of the user.</summary>
    public Status Status { get; set; }
}
