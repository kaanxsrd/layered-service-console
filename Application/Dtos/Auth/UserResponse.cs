// Output DTO describing a user without exposing credentials.
using Project.Domain.Enums;

namespace Project.Application.Dtos.Auth;

/// <summary>Public view of a user, safe to return across service boundaries.</summary>
/// <param name="Id">Unique identifier.</param>
/// <param name="Name">Display name.</param>
/// <param name="Email">Email address.</param>
/// <param name="Role">Assigned role.</param>
/// <param name="Status">Current account status.</param>
public sealed record UserResponse(
    Guid   Id,
    string Name,
    string Email,
    Role   Role,
    Status Status
);
