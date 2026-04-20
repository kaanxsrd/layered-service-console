// Privilege level assigned to a user.
namespace Project.Domain.Enums;

/// <summary>
/// Represents the user's privilege level within the system.
/// </summary>
public enum Role
{
    /// <summary>System administrator with full access.</summary>
    Root,

    /// <summary>Administrative user with management capabilities.</summary>
    Admin,

    /// <summary>Standard system user.</summary>
    User,
}
