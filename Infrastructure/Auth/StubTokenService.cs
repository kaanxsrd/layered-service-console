// TODO(auth): replace StubTokenService with a real token/session implementation
using Project.Application.Abstractions;
using Project.Domain.Entities;

namespace Project.Infrastructure.Auth;

/// <summary>
/// Placeholder <see cref="ITokenService"/> that returns an opaque <see cref="Guid"/>.
/// Swap for a real implementation (JWT or server-side session)
/// </summary>
public sealed class StubTokenService : ITokenService
{
    /// <inheritdoc />
    public Task<string> IssueAsync(User user, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(user);
        return Task.FromResult(Guid.NewGuid().ToString("N"));
    }
}
