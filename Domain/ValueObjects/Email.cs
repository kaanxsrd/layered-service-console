namespace Project.Domain.ValueObjects;

/// <summary>
/// An email address. Using this type instead of <c>string</c> makes it impossible to
/// accidentally pass an arbitrary string where a validated email is required.
/// Validation (non-empty, contains @) is the caller's responsibility before calling
/// <see cref="From"/>; this type guarantees identity and equality semantics.
/// </summary>
public readonly record struct Email(string Value)
{
    /// <summary>Wraps a pre-validated email string.</summary>
    public static Email From(string value) => new(value);

    /// <summary>Case-insensitive equality, matching standard email comparison rules.</summary>
    public bool Equals(string other) =>
        Value.Equals(other, StringComparison.OrdinalIgnoreCase);

    public override string ToString() => Value;
}
