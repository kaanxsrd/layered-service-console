// Abstraction for hashing and verifying passwords.
using Project.Domain.ValueObjects;

namespace Project.Application.Abstractions;

/// <summary>
/// Strategy for hashing and verifying user passwords. Implementations must use a
/// per-credential salt and a slow KDF (e.g. PBKDF2, Argon2, bcrypt). Never store or
/// compare plaintext passwords.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>Hashes <paramref name="plaintext"/> with a freshly generated salt.</summary>
    /// <param name="plaintext">The plaintext password to hash.</param>
    /// <returns>The resulting <see cref="PasswordHash"/>.</returns>
    PasswordHash Hash(string plaintext);

    /// <summary>Verifies <paramref name="plaintext"/> against a previously computed hash.</summary>
    /// <param name="plaintext">The candidate plaintext password.</param>
    /// <param name="hash">The stored hash to compare against.</param>
    /// <returns><c>true</c> when the plaintext matches the hash; otherwise <c>false</c>.</returns>
    bool Verify(string plaintext, PasswordHash hash);
}
