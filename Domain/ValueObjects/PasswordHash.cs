// Immutable representation of a hashed password and its salt.
namespace Project.Domain.ValueObjects;

/// <summary>
/// Represents a hashed password together with the per-user salt used to produce it.
/// Never stores plaintext credentials.
/// </summary>
/// <param name="Hash">The derived hash bytes, Base64 encoded.</param>
/// <param name="Salt">The per-user salt bytes, Base64 encoded.</param>
/// <param name="Iterations">Number of iterations used by the KDF.</param>
public readonly record struct PasswordHash(string Hash, string Salt, int Iterations)
{
    /// <summary>An empty/default hash. Use only as a placeholder; never for authentication.</summary>
    public static PasswordHash Empty { get; } = new(string.Empty, string.Empty, 0);

    /// <summary>True when all fields are default/empty.</summary>
    public bool IsEmpty => string.IsNullOrEmpty(Hash) && string.IsNullOrEmpty(Salt) && Iterations == 0;
}
