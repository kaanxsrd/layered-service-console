// Input DTO for registering a new user.
using Project.Domain.Enums;

namespace Project.Application.Dtos.Auth;

/// <summary>Fields required to register a new user.</summary>
/// <param name="Name">Display name.</param>
/// <param name="Email">Email address (must be unique).</param>
/// <param name="Password">Plaintext password; hashed before storage.</param>
/// <param name="Role">Role to assign to the new user.</param>
public sealed record RegisterUserRequest(
    string Name,
    string Email,
    string Password,
    Role   Role
);
