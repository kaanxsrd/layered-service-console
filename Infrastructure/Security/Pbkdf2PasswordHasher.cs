// PBKDF2-SHA256 password hasher with per-user salt and constant-time verification.
using System.Security.Cryptography;
using Project.Application.Abstractions;
using Project.Domain.ValueObjects;

namespace Project.Infrastructure.Security;

/// <summary>
/// PBKDF2 password hasher using HMAC-SHA256. Each hash uses a fresh 128-bit salt and
/// 100,000 iterations. Verification is constant-time.
/// </summary>
public sealed class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int SaltSizeBytes = 16;
    private const int HashSizeBytes = 32;
    private const int Iterations    = 100_000;

    /// <inheritdoc />
    public PasswordHash Hash(string plaintext)
    {
        ArgumentNullException.ThrowIfNull(plaintext);

        byte[] salt = RandomNumberGenerator.GetBytes(SaltSizeBytes);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            plaintext,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSizeBytes);

        return new PasswordHash(
            Hash:       Convert.ToBase64String(hash),
            Salt:       Convert.ToBase64String(salt),
            Iterations: Iterations);
    }

    /// <inheritdoc />
    public bool Verify(string plaintext, PasswordHash hash)
    {
        if (plaintext is null || hash.IsEmpty) return false;

        byte[] salt     = Convert.FromBase64String(hash.Salt);
        byte[] expected = Convert.FromBase64String(hash.Hash);
        byte[] actual   = Rfc2898DeriveBytes.Pbkdf2(
            plaintext,
            salt,
            hash.Iterations,
            HashAlgorithmName.SHA256,
            expected.Length);

        return CryptographicOperations.FixedTimeEquals(actual, expected);
    }
}
