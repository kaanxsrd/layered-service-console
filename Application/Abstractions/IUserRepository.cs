// Abstraction over the user store; swappable between in-memory and EF Core implementations.
using Project.Domain.Entities;

namespace Project.Application.Abstractions;

/// <summary>
/// Persistence contract for <see cref="User"/> aggregates. Implementations may be in
/// memory, relational, or otherwise; all methods are async and accept a
/// <see cref="CancellationToken"/> to match the future EF Core surface.
/// </summary>
public interface IUserRepository
{
    /// <summary>Fetches a user by its unique identifier.</summary>
    /// <param name="id">The user identifier.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user or <c>null</c> when no match exists.</returns>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>Fetches a user by email (case-insensitive match).</summary>
    /// <param name="email">The email address to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user or <c>null</c> when no match exists.</returns>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    /// <summary>Returns all users in the store.</summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All users, in no guaranteed order.</returns>
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>Persists a new user.</summary>
    /// <param name="user">The user to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AddAsync(User user, CancellationToken cancellationToken);

    /// <summary>Updates an existing user in place.</summary>
    /// <param name="user">The user with updated values.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task UpdateAsync(User user, CancellationToken cancellationToken);

    /// <summary>Tests whether a user with the given email already exists.</summary>
    /// <param name="email">The email address to test (case-insensitive).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns><c>true</c> when a match exists; otherwise <c>false</c>.</returns>
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
}
