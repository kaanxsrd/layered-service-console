// Public contract for authentication and user-management operations.
using Project.Application.Common;
using Project.Application.Dtos.Auth;
using Project.Domain.Enums;

namespace Project.Application.Services.Auth;

/// <summary>
/// Coordinates login, registration, logout, and simple user administration.
/// Maintains the currently logged-in user for the process lifetime.
/// </summary>
public interface IAuthService
{
    /// <summary>The currently logged-in user, or <c>null</c> when no one is signed in.</summary>
    UserResponse? CurrentUser { get; }

    /// <summary>True when <see cref="CurrentUser"/> is populated.</summary>
    bool IsLoggedIn { get; }

    /// <summary>Authenticates a caller by credentials and issues a session token.</summary>
    /// <param name="request">The login request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);

    /// <summary>Registers a new user with a hashed password.</summary>
    /// <param name="request">The registration request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<Result<UserResponse>> RegisterAsync(RegisterUserRequest request, CancellationToken cancellationToken);

    /// <summary>Lists all users, projected to their public DTO form.</summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<UserResponse>> ListUsersAsync(CancellationToken cancellationToken);

    /// <summary>Updates the status of a target user, subject to authorization rules.</summary>
    /// <param name="targetUserId">The id of the user whose status to change.</param>
    /// <param name="newStatus">The new status.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<Result<UserResponse>> SetStatusAsync(Guid targetUserId, Status newStatus, CancellationToken cancellationToken);

    /// <summary>Clears the current user from the session.</summary>
    void Logout();
}
