// Output DTO returned after a successful login.
namespace Project.Application.Dtos.Auth;

/// <summary>The result of a successful login.</summary>
/// <param name="User">The authenticated user's public details.</param>
/// <param name="Token">The issued session token.</param>
public sealed record LoginResponse(UserResponse User, string Token);
