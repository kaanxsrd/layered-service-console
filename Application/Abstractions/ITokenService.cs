// Abstraction for issuing authentication tokens after a successful login.
using Project.Domain.Entities;

namespace Project.Application.Abstractions;

/// <summary>
/// Issues authentication tokens after a successful login. The current stub returns an
/// opaque <see cref="System.Guid"/>; production implementations should return a signed
/// token (JWT, opaque session ID backed by a store, etc.).
/// </summary>
public interface ITokenService
{
    /// <summary>Issues a token for the given authenticated user.</summary>
    /// <param name="user">The authenticated user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>An opaque token string.</returns>
    Task<string> IssueAsync(User user, CancellationToken cancellationToken);
}
