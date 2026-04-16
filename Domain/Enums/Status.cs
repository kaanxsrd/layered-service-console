// Account activation status for a user.
namespace Project.Domain.Enums;

/// <summary>
/// Status of a user account.
/// </summary>
public enum Status
{
    /// <summary>The account is active and can log in.</summary>
    Active,

    /// <summary>The account is inactive but still existing.</summary>
    Inactive,

    /// <summary>The account is disabled and cannot be used.</summary>
    Disabled,
}
