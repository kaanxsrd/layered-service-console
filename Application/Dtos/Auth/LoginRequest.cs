// Input DTO for authenticating a user by email + password.
namespace Project.Application.Dtos.Auth;

/// <summary>Credentials supplied by a caller attempting to log in.</summary>
/// <param name="Email">The user's email address.</param>
/// <param name="Password">The user's plaintext password, transferred over trust boundary only.</param>
public sealed record LoginRequest(string Email, string Password);
